namespace WhoWantsToBeAMillionaire.Models.Game
{
    public class GameRound
    {
        public int RoundId { get; set; }
        public int GameId { get; set; }
        public int QuestionId { get; set; }
        public int? AnswerId { get; set; }
        public int Duration { get; set; }
        public bool UsedJoker { get; set; }
    }
}