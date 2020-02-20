using System;
using System.Collections.Generic;
using WhoWantsToBeAMillionaire.Models.Lifecycle.Users;

namespace WhoWantsToBeAMillionaire.Models.Lifecycle.Games
{
    public class GameManager
    {
        private readonly List<RunningGame> _runningGames = new List<RunningGame>();
        private readonly UserManager _userManager;

        public GameManager(UserManager userManager)
        {
            _userManager = userManager;
        }

        public string StartGame(UserCredentials credentials, IEnumerable<int> categories)
        {
            var gameId = Guid.NewGuid().ToString();
            _runningGames.Add(new RunningGame(gameId, categories));
            return gameId;
        }
    }
}