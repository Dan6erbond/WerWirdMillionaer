using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WhoWantsToBeAMillionaire.Models;
using WhoWantsToBeAMillionaire.Models.Api.ApiErrors.Users;
using WhoWantsToBeAMillionaire.Models.Data.Users;
using WhoWantsToBeAMillionaire.Models.Exceptions;
using WhoWantsToBeAMillionaire.Models.Lifecycle.Users;

namespace WhoWantsToBeAMillionaire.Controllers
{
    [Route("api/users")]
    public class UserController : Controller
    {
        private readonly UserManager _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserController(UserManager userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        [Authorize]
        [HttpGet("data")]
        public IActionResult GetUser()
        {
            var username = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _userManager.GetUser(username);
            return Ok(user);
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] UserCredentials credentials)
        {
            try
            {
                _userManager.CreateUser(credentials);
                return Ok();
            }
            catch (UserAlreadyExistsException e)
            {
                return BadRequest(new UserAlreadyExistsError(e.Message));
            }
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserCredentials credentials)
        {
            try
            {
                var token = _userManager.LoginUser(credentials);
                
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            catch (IncorrectPasswordException e)
            {
                return BadRequest(new IncorrectPasswordError(e.Message));
            }
            catch (UserDoesNotExistException e)
            {
                return BadRequest(new UserDoesNotExistError(e.Message));
            }
        }
    }
}