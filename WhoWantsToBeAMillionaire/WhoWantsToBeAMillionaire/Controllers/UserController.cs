using Microsoft.AspNetCore.Mvc;
using WhoWantsToBeAMillionaire.Models;
using WhoWantsToBeAMillionaire.Models.Api.ApiErrors.Users;
using WhoWantsToBeAMillionaire.Models.Exceptions;
using WhoWantsToBeAMillionaire.Models.Users;

namespace WhoWantsToBeAMillionaire.Controllers
{
    [Route("api/users")]
    public class UserController : Controller
    {
        private readonly IRepository<User> _userRepository;
        private readonly UserManager _userManager;

        public UserController(IRepository<User> userRepository, UserManager userManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
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
                    token = _userManager.LogInUser(credentials)
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