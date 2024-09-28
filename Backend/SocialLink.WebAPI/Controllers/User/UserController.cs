using EventBus;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SocialLink.Domain;
using SocialLink.Domain.Entities;
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
        private readonly IUserRepository repository;
        private readonly UserDomainService userDomainService;

        public UserController(UserDomainService userDomainService, IUserRepository repository, IEventBus eventBus)
        {
            this.userDomainService = userDomainService;
            this.repository = repository;
            this.eventBus = eventBus;
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteAdminUser(UserId id)
        {
            await repository.RemoveUserAsync(id);
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<UserResponse>> GetUserInfo()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId.IsNullOrEmpty())
                return NotFound();
            var user = await repository.FindByIdAsync(UserId.Parse(userId!));
            if (user == null)
                return NotFound();

            return new UserResponse(user.Id, user.UserName!, user.CreationTime);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login-by-name-and-password")]
        public async Task<ActionResult<string?>> LoginByNameAndPassword(NameAndPasswordRequest request)
        {
            (var checkResult, var token) = await userDomainService.LoginByNameAndPasswordAsync(request.Name, request.Password);
            if (checkResult.Succeeded) return token;
            else if (checkResult.IsLockedOut)//尝试登录次数太多
                return StatusCode((int)HttpStatusCode.Locked, "用户已经被锁定");
            else
            {
                string msg = checkResult.ToString();
                return BadRequest("登录失败" + msg);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("register-user")]
        public async Task<ActionResult> RegisterUser(NameAndPasswordRequest request)
        {
            var user = await repository.FindByNameAsync(request.Name);

            if (user != null)
            {
                return Created();
            }

            (var result, var resultUser) = await repository.RegisterAsync(request.Name, request.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.SumErrors());
            }
            var userCreatedEvent = new UserCreatedEvent(resultUser.Id, request.Name, request.Password);
            eventBus.Publish("UserService.User.Created", userCreatedEvent);

            return Ok();
        }
    }
}