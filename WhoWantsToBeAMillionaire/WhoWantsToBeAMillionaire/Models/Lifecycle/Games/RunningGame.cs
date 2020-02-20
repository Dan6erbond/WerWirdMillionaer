using System.Collections.Generic;
using System.Linq;

namespace WhoWantsToBeAMillionaire.Models.Lifecycle.Games
{
    public class RunningGame
    {
        public string GameId { get; }
        public List<int> Categories { get; }

        public RunningGame(string gameId, IEnumerable<int> categories)
        {
            GameId = gameId;
            Categories = categories.ToList();
        }
    }
}