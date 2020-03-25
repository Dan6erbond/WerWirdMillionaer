using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace WhoWantsToBeAMillionaire.Models.Data.Games
{
    public class Game
    {
        /* Data present in DB */
        public int GameId { get; set; }
        [JsonIgnore] public int UserId { get; set; }
        public DateTime Start { get; set; }
        public bool Hidden { get; set; }
        public string Username { get; set; }

        /* Data filled by logic */
        public int Duration { get; set; }
        public List<Round> Rounds { get; set; } = new List<Round>();
        public int Points { get; set; }
        public int WeightedPoints { get; set; }
        public int? Rank { get; set; }
        public List<int> Categories { get; set; } = new List<int>();

        public Game()
        {
        }

        public Game(int gameId, int userId, DateTime start, bool hidden = false)
        {
            GameId = gameId;
            UserId = userId;
            Start = start;
            Hidden = hidden;
        }
    }
}