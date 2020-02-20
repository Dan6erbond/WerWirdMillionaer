using System.Collections.Generic;
using System.Linq;

namespace WhoWantsToBeAMillionaire.Models.Lifecycle.Games
{
    public class RunningGame
    {
        public string GameId { get; }
        public int UserId { get; }
        public List<int> Categories { get; }
        public List<Question> AskedQuestions { get; } = new List<Question>();
        public Question CurrentQuestion { get; set; }

        public RunningGame(int userId, string gameId, IEnumerable<int> categories)
        {
            UserId = userId;
            GameId = gameId;
            Categories = categories.ToList();
        }
    }
}