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

        public QuizQuestion()
        {
        }

        public QuizQuestion(int questionId, int categoryId, string question)
        {
            QuestionId = questionId;
            CategoryId = categoryId;
            Question = question;
        }

        public QuizQuestion(string question, params string[] answers)
        {
            Question = question;
            Answers.Add(new QuizAnswer(answers[0], true));
            for (int i = 1; i < 4; i++)
            {
                Answers.Add(new QuizAnswer(answers[i]));
            }
        }
    }
}