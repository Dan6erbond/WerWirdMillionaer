using Newtonsoft.Json;

namespace WhoWantsToBeAMillionaire.Models.Data.Games
{
    public class CategoryGame
    {
        public int CategoryGameId { get; set; }
        public int CategoryId { get; set; }
        public int GameId { get; set; }

        public CategoryGame()
        {
        }

        public CategoryGame(int categoryGameId, int categoryId, int gameId)
        {
            CategoryGameId = categoryGameId;
            CategoryId = categoryId;
            GameId = gameId;
        }

        public CategoryGame(int categoryId, int gameId)
        {
            CategoryId = categoryId;
            GameId = gameId;
        }
    }
}