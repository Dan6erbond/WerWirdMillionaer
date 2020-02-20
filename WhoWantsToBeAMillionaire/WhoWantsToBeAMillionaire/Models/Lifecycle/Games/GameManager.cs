using System;
using System.Collections.Generic;
using WhoWantsToBeAMillionaire.Models.Data.Users;
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

        public string StartGame(User user, IEnumerable<int> categories)
        {
            var gameId = Guid.NewGuid().ToString();
            _runningGames.Add(new RunningGame(user.UserId, gameId, categories));
            return gameId;
        }
    }
}