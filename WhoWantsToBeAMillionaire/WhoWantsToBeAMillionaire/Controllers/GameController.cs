using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.WebEncoders.Testing;
using WhoWantsToBeAMillionaire.Models;
using WhoWantsToBeAMillionaire.Models.Data.Games;
using WhoWantsToBeAMillionaire.Models.Data.Quiz;
using WhoWantsToBeAMillionaire.Models.Data.Users;
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
        private readonly IRepository<Game> _gameRepository;
        private readonly IRepository<Round> _roundRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<QuizAnswer> _answerRepository;

        private readonly GameManager _gameManager;
        private readonly UserManager _userManager;

        public GameController(IHttpContextAccessor httpContextAccessor, IRepository<Category> categoryRepository,
            IRepository<Game> gameRepository, IRepository<Round> roundRepository, GameManager gameManager,
            IRepository<User> userRepository, IRepository<QuizAnswer> answerRepository, UserManager userManager)
        {
            _httpContextAccessor = httpContextAccessor;

            _categoryRepository = categoryRepository;
            _gameRepository = gameRepository;
            _roundRepository = roundRepository;
            _userRepository = userRepository;
            _answerRepository = answerRepository;

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

        [AllowAnonymous]
        [HttpGet("leaderboard")]
        public IActionResult GetRounds()
        {
            var games = _gameRepository.List.ToList();

            foreach (var game in games)
            {
                var userSpecification = new UserSpecification(game.UserId);
                var user = _userRepository.Query(userSpecification).First();
                game.Username = user.Username;

                var roundSpecification = new RoundSpecification(game.GameId);
                var rounds = _roundRepository.Query(roundSpecification);
                game.Rounds = rounds;

                foreach (var round in rounds)
                {
                    game.Duration += round.Duration;

                    var answerSpecification = new QuizAnswerSpecification(round.AnswerId);
                    var answer = _answerRepository.Query(answerSpecification).FirstOrDefault();
                    if (answer != null && answer.Correct)
                    {
                        game.Points += 30;
                    }
                }

                game.WeightedPoints = game.Points / game.Duration;
            }

            games.Sort((game, game1) => game.CompareTo(game1));

            for (int i = 0; i < games.Count; i++)
            {
                games[i].Rank = i + 1;
            }

            return Ok(games);
        }
    }
}