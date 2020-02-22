using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.WebEncoders.Testing;
using WhoWantsToBeAMillionaire.Models;
using WhoWantsToBeAMillionaire.Models.Data.Quiz;
using WhoWantsToBeAMillionaire.Models.Lifecycle.Games;
using WhoWantsToBeAMillionaire.Models.Lifecycle.Users;

namespace WhoWantsToBeAMillionaire.Controllers
{
    [Authorize]
    [Route("api/games")]
    public class GameController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IRepository<Category> _categoryRepository;

        private readonly GameManager _gameManager;
        private readonly UserManager _userManager;

        public GameController(IHttpContextAccessor httpContextAccessor, IRepository<Category> categoryRepository,
            GameManager gameManager, UserManager userManager
        )
        {
            _httpContextAccessor = httpContextAccessor;

            _categoryRepository = categoryRepository;

            _gameManager = gameManager;
            _userManager = userManager;
        }

        [HttpGet("start")]
        public IActionResult StartGame([FromBody] GameSpecification specification)
        {
            var username = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _userManager.GetUser(username);

            var gameId = _gameManager.StartGame(user, specification.Categories);

            return Ok(new
            {
                gameId = gameId
            });
        }

        [HttpGet("categories")]
        public IEnumerable<Category> GetCategories()
        {
            return _categoryRepository.List;
        }
    }
}