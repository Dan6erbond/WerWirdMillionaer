using Newtonsoft.Json;

namespace WhoWantsToBeAMillionaire.Models.Data.Games
{
    public class CategoryGame
    {
        public int CategoryGameId { get; set; }
        public int CategoryId { get; set; }
        public int GameId { get; set; }

        public CategoryGame(int categoryId, int gameId)
        {
            CategoryId = categoryId;
            GameId = gameId;
        }
    }
}