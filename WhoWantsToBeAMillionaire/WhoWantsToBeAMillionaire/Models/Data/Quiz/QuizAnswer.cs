namespace WhoWantsToBeAMillionaire.Models.Data.Quiz
{
    public class QuizAnswer : IAnswer
    {
        public int AnswerId { get; set; }
        public int QuestionId { get; set; }
        public string Answer { get; set; }
        public bool Correct { get; set; }

        public QuizAnswer()
        {
        }

        public QuizAnswer(int answerId, int questionId, string answer, bool correct = false)
        {
            AnswerId = answerId;
            QuestionId = questionId;
            Answer = answer;
            Correct = correct;
        }
        
        public QuizAnswer(string answer, bool correct = false)
        {
            Answer = answer;
            Correct = correct;
        }
    }
}