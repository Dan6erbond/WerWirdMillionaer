using Newtonsoft.Json;

namespace WhoWantsToBeAMillionaire.Models.Data.Games
{
    public class Round
    {
        public int RoundId { get; set; }
        [JsonIgnore] public int GameId { get; set; }
        public int QuestionId { get; set; }
        public int? AnswerId { get; set; }
        public int Duration { get; set; }
        public bool UsedJoker { get; set; }
    }
}