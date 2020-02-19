namespace WhoWantsToBeAMillionaire.Models.Quiz
{
    public class QuizQuestion
    {
        public int QuestionId { get; set; }
        public int CategoryId { get; set; }
        public string Question { get; set; }
    }
}