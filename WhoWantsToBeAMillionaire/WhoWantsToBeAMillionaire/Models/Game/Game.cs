using System;

namespace WhoWantsToBeAMillionaire.Models.Game
{
    public class Game
    {
        public int GameId { get; set; }
        public int UserId { get; set; }
        public DateTime Start { get; set; }
    }
}