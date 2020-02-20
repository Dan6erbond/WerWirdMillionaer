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

        public UserController(UserManager userManager)
        {
            _userManager = userManager;
        }

        [HttpGet("data")]
        public IActionResult GetUser([FromBody] UserCredentials credentials)
        {
            try
            {
                var user = _userManager.GetUser(credentials.Token);
                return Ok(user);
            }
            catch (InvalidTokenException e)
            {
                return BadRequest(new InvalidTokenError(e.Message));
            }
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

        [HttpGet("login")]
        public IActionResult Login([FromBody] UserCredentials credentials)
        {
            try
            {
                return Ok(new
                {
                    token = _userManager.LogInUser(credentials),
                    expires = "1 hour"
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