using System;
using System.Text.Json.Serialization;

namespace WhoWantsToBeAMillionaire.Models.Data.Quiz
{
    public class QuizAnswer
    {
        public int AnswerId { get; set; }
        [JsonIgnore] public int QuestionId { get; set; }
        public string Answer { get; set; }
        public bool Correct { get; set; }
        [JsonIgnore] public bool HiddenAnswer { get; set; } = false;

        public QuizAnswer()
        {
        }

        public QuizAnswer(string answer, bool correct = false)
        {
            Answer = answer;
            Correct = correct;
        }

        // expose Correct property to API so false answers can be hidden when Joker is used
        public bool ShouldSerializeCorrect() => HiddenAnswer;
    }
}