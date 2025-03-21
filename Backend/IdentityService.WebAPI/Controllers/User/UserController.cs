﻿using DomainCommons.EntityStronglyIds;
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
using System.Net;
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
    public async Task<ActionResult> Delete()
    {
        if (TryFindUserId(out var userId))
        {
            return Unauthorized();
        }
        var result = await userRepository.RemoveUserAsync(userId);
        if (!result.Succeeded)
            return BadRequest();
        return Ok();
    }

    [AllowAnonymous]
    [HttpGet]
    [Route("get-email-reset-code")]
    public async Task<ActionResult> GetEmailResetCode([EmailAddress][Required] string email)
    {
        var result = await userDomainService.GetEmailResetCode(email);
        if (!result.IdentityResult.Succeeded)
        {
            return NotFound(result.IdentityResult.Errors.SumErrors());
        }
        await emailSender.SendEmailAsync(result.Email, result.Subject, result.HtmlMessage);
        return Ok("Email reset code has been sent.");
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<GetUserInfoResponse>> GetUserInfo()
    {
        if (TryFindUserId(out var userId))
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
    [HttpPost]
    [Route("login-by-email-and-password")]
    public async Task<ActionResult<string?>> LoginByEmailAndPassword(LoginByEmailAndPasswordRequest request)
    {
        var loginResult = await userDomainService.LoginByEmailAndPasswordAsync(request.Email, request.Password);
        return HandleLoginResult(loginResult);
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("login-by-name-and-password")]
    public async Task<ActionResult<string?>> LoginByNameAndPassword(LoginByNameAndPasswordRequest request)
    {
        var loginResult = await userDomainService.LoginByNameAndPasswordAsync(request.Name, request.Password);
        return HandleLoginResult(loginResult);
    }

    [Authorize]
    [HttpPut]
    [Route("reset-password")]
    public async Task<ActionResult> ResetPassword([Required] string oldPassword, [Required] string newPassword)
    {
        if (TryFindUserId(out var userId))
        {
            return Unauthorized();
        }

        var result = await userRepository.ResetPasswordByIdAsync(userId, oldPassword, newPassword);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors.SumErrors());
        }
        return Ok(new { Message = "Password reset successful." });
    }

    [AllowAnonymous]
    [HttpPut]
    [Route("reset-password-with-email-code")]
    public async Task<ActionResult> ResetPasswordWithEmailCode(ResetPasswordRequest resetPasswordRequest)
    {
        var user = await userRepository.FindByEmailAsync(resetPasswordRequest.Email);
        if (user == null)
        {
            return NotFound(new
            {
                error = "UserNotFound",
                message = "The user with the specified email does not exist."
            });
        }
        var result = await userDomainService.ResetPasswordByEmailResetCodeAsync(resetPasswordRequest);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors.SumErrors());
        }
        return Ok(new { Message = "Password reset successful." });
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("sign-up")]
    public async Task<ActionResult> SignUp(SignUpRequest request)
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

    private ActionResult<string?> HandleLoginResult(LoginResult loginResult)
    {
        if (loginResult.IsSuccess) return loginResult.Token;
        else if (loginResult.SignInResult.IsLockedOut)
            return StatusCode((int)HttpStatusCode.Locked, "User account is locked due to too many failed login attempts.");
        else
        {
            string msg = loginResult.SignInResult.ToString();
            return Unauthorized("Login failed \n" + msg);
        }
    }

    private bool TryFindUserId(out UserId userId)
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
