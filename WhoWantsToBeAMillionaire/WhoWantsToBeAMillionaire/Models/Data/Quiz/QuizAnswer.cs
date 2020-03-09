using System.Text.Json.Serialization;

namespace WhoWantsToBeAMillionaire.Models.Data.Quiz
{
    public class QuizAnswer
    {
        public int AnswerId { get; set; }
        [JsonIgnore] public int QuestionId { get; set; }
        public string Answer { get; set; }
        public bool Correct { get; set; }

        public QuizAnswer()
        {
        }

        public QuizAnswer(string answer, bool correct = false)
        {
            Answer = answer;
            Correct = correct;
        }
    }
}