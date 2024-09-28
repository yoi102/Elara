using EventBus;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SocialLink.Domain;
using SocialLink.Domain.DomainService;
using SocialLink.Domain.Entities;
using SocialLink.Domain.Results;
using SocialLink.WebAPI.Controllers.User.Request;
using SocialLink.WebAPI.Controllers.User.Response;
using SocialLink.WebAPI.Events;
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
        public UserController(IUserDomainService userDomainService, IUserRepository repository, IEventBus eventBus)
        {
            this.userDomainService = userDomainService;
            this.userRepository = repository;
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
            await userRepository.RemoveUserAsync(UserId.Parse(userId!));
            return Ok();
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserResponse>> GetUserInfo()
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
            return new UserResponse(user.Id, user.UserName, user.CreationTime);
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
        public async Task<ActionResult<string?>> LoginByNameAndPassword(NameAndPasswordRequest request)
        {
            var loginResult = await userDomainService.LoginByNameAndPasswordAsync(request.Name, request.Password);
            return HandleLoginResult(loginResult);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login-by-phone-number-and-password")]
        public async Task<ActionResult<string?>> LoginByPhoneNumberAndPassword(NameAndPasswordRequest request)
        {
            var loginResult = await userDomainService.LoginByPhoneNumberAndPasswordAsync(request.Name, request.Password);
            return HandleLoginResult(loginResult);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("register-user")]
        public async Task<ActionResult> RegisterUser(NameAndPasswordRequest request)
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

            (var result, var resultUser) = await userRepository.RegisterAsync(request.Name, request.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.SumErrors());
            }
            var userCreatedEvent = new UserCreatedEvent(resultUser.Id, request.Name, request.Password);
            eventBus.Publish("UserService.User.Created", userCreatedEvent);

            return Created();
        }

        private ActionResult<string?> HandleLoginResult(LoginResult loginResult)
        {
            if (loginResult.IsSuccess) return loginResult.Token;
            else if (loginResult.SignInResult.IsLockedOut)//尝试登录次数太多
                return StatusCode((int)HttpStatusCode.Locked, "用户已经被锁定");
            else
            {
                string msg = loginResult.SignInResult.ToString();
                return BadRequest("登录失败" + msg);
            }
        }
    }
}