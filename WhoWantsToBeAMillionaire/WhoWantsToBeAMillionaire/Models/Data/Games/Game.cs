using System;

namespace WhoWantsToBeAMillionaire.Models.Data.Games
{
    public class Game
    {
        public int GameId { get; set; }
        public int UserId { get; set; }
        public DateTime Start { get; set; }
    }
}