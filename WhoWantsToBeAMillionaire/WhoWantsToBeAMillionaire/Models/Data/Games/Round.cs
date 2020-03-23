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

        public Round()
        {
        }

        public Round(int roundId, int gameId, int questionId, int duration, int? answerId = null, bool usedJoker = false)
        {
            RoundId = roundId;
            GameId = gameId;
            QuestionId = questionId;
            Duration = duration;
            AnswerId = answerId;
            UsedJoker = usedJoker;
        }
    }
}