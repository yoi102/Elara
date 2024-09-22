using DomainCommons.EntityStronglyIds;
using EventBus;
using IdentityService.Domain;
using IdentityService.Domain.Interfaces;
using IdentityService.Domain.Results;
using IdentityService.WebAPI.Controllers.User.Request;
using IdentityService.WebAPI.Controllers.User.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace IdentityService.WebAPI.Controllers.User;

[Route("api/users")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> logger;
    private readonly IUserDomainService userDomainService;
    private readonly IUserRepository userRepository;
    private readonly IEmailSender emailSender;
    private readonly IEventBus eventBus;

    public UserController(ILogger<UserController> logger,
        IUserDomainService userDomainService,
        IUserRepository userRepository,
        IEmailSender emailSender,
        IEventBus eventBus)
    {
        this.logger = logger;
        this.userDomainService = userDomainService;
        this.userRepository = userRepository;
        this.emailSender = emailSender;
        this.eventBus = eventBus;
    }

    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> Delete()
    {
        if (!TryFindUserIdFromClaims(out var userId))
        {
            return Unauthorized();
        }
        var result = await userRepository.RemoveUserAsync(userId);
        if (!result.Succeeded)
            return BadRequest();
        await eventBus.PublishAsync("IdentityService.UserCreated", new { UserId = userId });
        return Ok();
    }

    [AllowAnonymous]
    [HttpGet("get-reset-code-by-email")]
    public async Task<IActionResult> GetResetCodeByEmail([EmailAddress][Required] string email)
    {
        var result = await userDomainService.GetResetCodeByEmail(email);
        if (!result.IdentityResult.Succeeded)
        {
            return NotFound(result.IdentityResult.Errors.SumErrors());
        }
        await emailSender.SendEmailAsync(result.Email, result.Subject, result.HtmlMessage);

        var resetCode = result.HtmlMessage;
        return Ok(new { ResetCode = resetCode });//不应返回的，由于没EmailSender，无奈之举
        //return Ok("Email reset code has been sent.");
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<GetUserInfoResponse>> GetUserInfo()
    {
        if (!TryFindUserIdFromClaims(out var userId))
        {
            return Unauthorized();
        }
        var user = await userRepository.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound(new
            {
                error = "UserNotFound",
                message = "The user with the specified ID does not exist."
            });
        }

        if (user.UserName == null)
        {
            throw new ArgumentNullException(nameof(user.UserName), "UserName cannot be null");
        }
        return new GetUserInfoResponse(user.Id, user.UserName, user.Email, user.PhoneNumber, user.CreatedAt);
    }

    [AllowAnonymous]
    [HttpPost("login-by-email-and-password")]
    public async Task<IActionResult> LoginByEmailAndPassword(LoginByEmailAndPasswordRequest request)
    {
        var loginResult = await userDomainService.LoginByEmailAndPasswordAsync(request.Email, request.Password, request.UserAgent);
        if (!loginResult.IsSuccess)
        {
            return Unauthorized();
        }

        return Ok(new { loginResult.UserId, loginResult.UserName, loginResult.Token, loginResult.RefreshToken });
    }

    [AllowAnonymous]
    [HttpPost("login-by-name-and-password")]
    public async Task<IActionResult> LoginByNameAndPassword(LoginByNameAndPasswordRequest request)
    {
        var loginResult = await userDomainService.LoginByNameAndPasswordAsync(request.Name, request.Password, request.UserAgent);
        if (!loginResult.IsSuccess)
        {
            return Unauthorized();
        }

        return Ok(new { loginResult.UserId, loginResult.UserName, loginResult.Token, loginResult.RefreshToken });
    }

    [Authorize]
    [HttpPut("reset-password")]
    public async Task<IActionResult> ResetPassword([Required] string oldPassword, [Required] string newPassword)
    {
        if (!TryFindUserIdFromClaims(out var userId))
        {
            return Unauthorized();
        }

        var result = await userRepository.ResetPasswordByOldPasswordAsync(userId, oldPassword, newPassword);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors.SumErrors());
        }
        return Ok(new { Message = "Password reset successful." });
    }

    [AllowAnonymous]
    [HttpPut("reset-password-with-email-code")]
    public async Task<IActionResult> ResetPasswordWithEmailCode(ResetPasswordRequest request)
    {
        var user = await userRepository.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return NotFound(new
            {
                error = "UserNotFound",
                message = "The user with the specified email does not exist."
            });
        }
        var result = await userDomainService.ResetPasswordByResetCodeAsync(user, request.ResetCode, request.NewPassword);
        if (!result.Succeeded)
        {
            return BadRequest(new
            {
                error = "ValidationFailed",
                details = result.Errors.Select(e => new
                {
                    code = e.Code,
                    message = e.Description
                })
            });
        }
        return Ok(new { Message = "Password reset successful." });
    }

    [AllowAnonymous]
    [HttpPost("sign-up")]
    public async Task<IActionResult> SignUp(SignUpRequest request)
    {
        var user = await userRepository.FindByNameAsync(request.Name);

        if (user != null)
        {
            return Conflict(new
            {
                error = "UsernameAlreadyExists",
                message = "The username is already taken. Please choose a different one."
            });
        }
        var _user = await userRepository.FindByEmailAsync(request.Email);
        if (_user != null)
        {
            return Conflict(new
            {
                error = "UsernameAlreadyExists",
                message = "The username is already taken. Please choose a different one."
            });
        }

        var signUpResult = await userRepository.SignUpAsync(request.Name, request.Email, request.Password);
        if (!signUpResult.Succeeded)
        {
            return BadRequest(signUpResult.IdentityResult.Errors.SumErrors());
        }

        await eventBus.PublishAsync("IdentityService.UserCreated", new { UserId = signUpResult.User.Id.ToString(), signUpResult.User.UserName });

        return CreatedAtAction(nameof(SignUp), new { id = signUpResult.User.Id }, new { message = "User created successfully." });
    }

    [AllowAnonymous]
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        var result = await userDomainService.RefreshTokenAsync(request.UserId, request.RefreshToken, request.UserAgent);

        if (result.Status == RefreshTokenStatus.InvalidToken)
        {
            return Unauthorized("Refresh token not found or expired.");
        }
        else if (result.Status == RefreshTokenStatus.TokenMismatch)
        {
            return BadRequest("User ID does not match token data.");
        }
        else if (result.Status == RefreshTokenStatus.UserNotFound)
        {
            return BadRequest("Invalid refresh token.");
        }

        return Ok(new { request.UserId, result.UserName, result.Token, result.ReflashToken });
    }

    private bool TryFindUserIdFromClaims(out UserId userId)
    {
        userId = UserId.Empty;
        var stringUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(stringUserId))
        {
            return false;
        }
        if (!UserId.TryParse(stringUserId, out var result))
        {
            return false;
        }
        userId = result;
        return true;
    }
}
