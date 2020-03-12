using System;
using System.Collections.Generic;

namespace WhoWantsToBeAMillionaire.Models.Data.Quiz
{
    public class QuizQuestion : IQuestion<QuizAnswer>
    {
        public int QuestionId { get; set; }
        public int CategoryId { get; set; }
        public string Question { get; set; }
        public List<QuizAnswer> Answers { get; set; } = new List<QuizAnswer>();
    }
}