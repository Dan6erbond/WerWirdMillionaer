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
    [ApiController]
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

        [HttpPost("start")]
        public IActionResult StartGame([FromBody] GameSpecification specification)
        {
            var username = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _userManager.GetUser(username);

            _gameManager.StartGame(user, specification.Categories);

            return Ok();
        }

        [HttpGet("end")]
        public IActionResult EndGame()
        {
            var username = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _userManager.GetUser(username);

            _gameManager.EndGame(user);

            return Ok();
        }
        
        [HttpGet("question")]
        public IActionResult GetQuestion()
        {
            var username = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _userManager.GetUser(username);
            
            return Ok(_gameManager.GetQuestion(user));
        }

        [HttpPost("answer")]
        public IActionResult AnswerQuestion([FromBody] AnswerSpecification specification)
        {
            var username = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _userManager.GetUser(username);

            var result = _gameManager.AnswerQuestion(user, specification.QuestionId, specification.AnswerId);
            
            return Ok(result);
        }

        [HttpGet("joker")]
        public IActionResult UseJoker()
        {
            var username = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _userManager.GetUser(username);

            return Ok(_gameManager.UseJoker(user));
        }
        
        [AllowAnonymous]
        [HttpGet("categories")]
        public IActionResult GetCategories()
        {
            return Ok(_categoryRepository.List);
        }
    }
}