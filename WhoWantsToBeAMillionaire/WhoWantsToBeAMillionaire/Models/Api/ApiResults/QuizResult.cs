namespace WhoWantsToBeAMillionaire.Models.Data.Quiz
{
    public class QuizResult
    {
        public string Type { get; } = "QUIZ_RESULT";
        public bool Won { get; set; }
        public int Points { get; set; }
        public int TimeElapsed { get; set; }
        public bool JokerUsed { get; set; }
        public string correctAnswer { get; set; }
    }
}