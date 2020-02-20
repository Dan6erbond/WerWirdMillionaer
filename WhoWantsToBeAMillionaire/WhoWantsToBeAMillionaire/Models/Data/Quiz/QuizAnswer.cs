namespace WhoWantsToBeAMillionaire.Models.Data.Quiz
{
    public class QuizAnswer
    {
        public int AnswerId { get; set; }
        public int QuestionId { get; set; }
        public string Answers { get; set; }
        public bool Correct { get; set; }
    }
}