using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.WebEncoders.Testing;
using WhoWantsToBeAMillionaire.Models;
using WhoWantsToBeAMillionaire.Models.Api.ApiErrors.Users;
using WhoWantsToBeAMillionaire.Models.Data.Quiz;
using WhoWantsToBeAMillionaire.Models.Exceptions;
using WhoWantsToBeAMillionaire.Models.Lifecycle.Admin;
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
        private readonly IRepository<QuizQuestion> _questionRepository;
        private readonly IRepository<QuizAnswer> _answerRepository;

        private readonly GameManager _gameManager;
        private readonly UserManager _userManager;
        private readonly DataManager _dataManager;

        public GameController(IHttpContextAccessor httpContextAccessor, IRepository<Category> categoryRepository,
            IRepository<QuizQuestion> questionRepository, IRepository<QuizAnswer> answerRepository,
            GameManager gameManager, DataManager dataManager, UserManager userManager)
        {
            _httpContextAccessor = httpContextAccessor;

            _categoryRepository = categoryRepository;
            _questionRepository = questionRepository;
            _answerRepository = answerRepository;

            _gameManager = gameManager;
            _userManager = userManager;
            _dataManager = dataManager;
        }

        [AllowAnonymous]
        [HttpGet("category/{id}")]
        public IActionResult GetQuestions(int id)
        {
            return Ok(_dataManager.GetQuestions(id));
        }

        [HttpPost("start")]
        public IActionResult StartGame([FromBody] StartGameSpecification specification)
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

            var result = _gameManager.EndGame(user);

            return Ok(result);
        }

        [HttpGet("question")]
        public IActionResult GetQuestion()
        {
            var username = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _userManager.GetUser(username);

            try
            {
                var question = _gameManager.GetQuestion(user);
                return Ok(question);
            }
            catch (NoMoreQuestionsException e)
            {
                return BadRequest(new NoMoreQuestionsError(e.Message));
            }
        }

        [AllowAnonymous]
        [HttpGet("questions/{id}")]
        public IActionResult GetQuestionData(int id)
        {
            var questionSpecification = new QuizQuestionSpecification(id);
            var question = _questionRepository.Query(questionSpecification).First();

            var answerSpecification = new QuizAnswerSpecification(questionId: id);
            var answers = _answerRepository.Query(answerSpecification);

            question.Answers = answers;

            return Ok(question);
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

            var question = _gameManager.UseJoker(user);

            return Ok(question);
        }

        [HttpGet("my")]
        public IActionResult GetUserGames()
        {
            var username = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _userManager.GetUser(username);

            var games = _gameManager.GetGames(user);

            return Ok(games);
        }

        [AllowAnonymous]
        [HttpGet("categories")]
        public IActionResult GetCategories()
        {
            var categories = _categoryRepository.List;
            return Ok(categories);
        }

        [AllowAnonymous]
        [HttpGet("leaderboard")]
        public IActionResult GetGames()
        {
            var games = _gameManager.GetGames();
            return Ok(games);
        }
    }
}