using EventBus;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SocialLink.Domain;
using SocialLink.Domain.Entities;
using SocialLink.Domain.Interfaces;
using SocialLink.Domain.Results;
using SocialLink.WebAPI.Controllers.User.Request;
using SocialLink.WebAPI.Controllers.User.Response;
using SocialLink.WebAPI.Events;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Security.Claims;

namespace SocialLink.WebAPI.Controllers.User
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IEventBus eventBus;
        private readonly IUserDomainService userDomainService;
        private readonly IUserRepository userRepository;

        public UserController(IUserDomainService userDomainService, IUserRepository userRepository, IEventBus eventBus)
        {
            this.userDomainService = userDomainService;
            this.userRepository = userRepository;
            this.eventBus = eventBus;
        }

        [Authorize]
        [HttpDelete]
        public async Task<ActionResult> Delete()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId.IsNullOrEmpty())
            {
                return NotFound(new
                {
                    error = "UserNotFound",
                    message = "The user with the specified ID does not exist."
                });
            }
            var result = await userRepository.RemoveUserAsync(UserId.Parse(userId!));
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
            var resetPasswordByEmailResetCodeEvent =
                new ResetPasswordByEmailResetCodeEvent(result.Email, result.Subject, result.HtmlMessage);

            eventBus.Publish("UserService.User.ResetUserPasswordByEmail", resetPasswordByEmailResetCodeEvent);
            return Ok("Email reset code has been sent.");
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<GetUserInfoResponse>> GetUserInfo()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId.IsNullOrEmpty())
            {
                return NotFound(new
                {
                    error = "Unauthorized",
                    message = "You must be logged in to perform this action."
                });
            }
            var user = await userRepository.FindByIdAsync(UserId.Parse(userId!));
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
            return new GetUserInfoResponse(user.Id, user.UserName, user.Email, user.PhoneNumber, user.CreationTime);
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
        [HttpPost]
        [Route("reset-password")]
        public async Task<ActionResult> ResetPassword([Required] string newPassword)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId.IsNullOrEmpty())
            {
                return NotFound(new
                {
                    error = "Unauthorized",
                    message = "You must be logged in to perform this action."
                });
            }

            var result = await userRepository.ResetPasswordByIdAsync(UserId.Parse(userId!), newPassword);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.SumErrors());
            }
            return Ok(new { Message = "Password reset successful." });
        }
        [AllowAnonymous]
        [HttpPost]
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
            var userCreatedEvent = new UserCreatedEvent(signUpResult.User.Id, request.Name, request.Password);
            eventBus.Publish("UserService.User.Created", userCreatedEvent);

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
    }
}