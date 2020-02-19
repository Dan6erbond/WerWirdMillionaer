using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WhoWantsToBeAMillionaire.Models;
using WhoWantsToBeAMillionaire.Models.Api.ApiErrors;
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
            var specification = new UserSpecification(username: credentials.Username);
            if (_userRepository.Query(specification).FirstOrDefault() != null)
            {
                var message = $"User {credentials.Username} already exists.";
                return BadRequest(new UserAlreadyExistsError(message));
            }

            PasswordHasher hasher = new PasswordHasher();
            hasher.GenerateSalt().HashPassword(hasher.Salt, credentials.Password);

            var user = new User(credentials.Username, hasher.Salt, hasher.Hashed);

            _userRepository.Create(user);

            return Ok();
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