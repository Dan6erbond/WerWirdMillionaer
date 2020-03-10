namespace WhoWantsToBeAMillionaire.Models.Data.Quiz
{
    public class QuizResult
    {
        public bool Won { get; set; }
        public int Points { get; set; }
        public long TimeElapsed { get; set; }
        public bool JokerUsed { get; set; }
    }
}