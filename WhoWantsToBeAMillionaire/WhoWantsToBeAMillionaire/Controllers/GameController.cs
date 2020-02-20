using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WhoWantsToBeAMillionaire.Models.Lifecycle.Games;
using WhoWantsToBeAMillionaire.Models.Lifecycle.Users;

namespace WhoWantsToBeAMillionaire.Controllers
{
    [Authorize]
    [Route("api/games")]
    public class GameController : Controller
    {
        private readonly GameManager _gameManager;
        private readonly UserManager _userManager;

        public GameController(GameManager gameManager, UserManager userManager)
        {
            _gameManager = gameManager;
            _userManager = userManager;
        }

        [HttpGet("start")]
        public IActionResult Start()
        {
            return null;
        }
    }
}