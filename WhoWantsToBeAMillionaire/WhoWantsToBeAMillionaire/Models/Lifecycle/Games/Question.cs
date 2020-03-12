using System;
using System.Collections.Generic;
using System.Linq;
using WhoWantsToBeAMillionaire.Models.Data.Quiz;

namespace WhoWantsToBeAMillionaire.Models.Lifecycle.Games
{
    public class Question
    {
        public QuizQuestion QuizQuestion { get; }
        public Answer AnsweredAnswer { get; set; }
        public bool JokerUsed { get; private set; } = false;
        public DateTime TimeAsked { get; set; }
        public DateTime TimeAnswered { get; set; }

        public Question(QuizQuestion quizQuestion)
        {
            QuizQuestion = quizQuestion;
        }

        public QuizQuestion UseJoker()
        {
            JokerUsed = true;

            var random = new Random();
            var indexes = new List<int>();
            for (int i = 0; i < 2; i++)
            {
                int index;
                do
                {
                    index = random.Next(QuizQuestion.Answers.Count);
                } while (QuizQuestion.Answers[index].Correct || indexes.Contains(index));
                
                indexes.Add(index);
                QuizQuestion.Answers[index].HiddenAnswer = true;
            }

            return QuizQuestion;
        }
    }
}