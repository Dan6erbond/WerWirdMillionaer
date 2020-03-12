using System;
using System.Collections.Generic;
using System.Linq;
using WhoWantsToBeAMillionaire.Models.Data.Quiz;

namespace WhoWantsToBeAMillionaire.Models.Lifecycle.Games
{
    public class RunningGame
    {
        public int UserId { get; }
        public List<int> Categories { get; }
        public List<Question> AskedQuestions { get; } = new List<Question>();
        public Question CurrentQuestion { get; set; }

        public bool JokerUsed
        {
            get
            {
                var jokerUsed = CurrentQuestion.JokerUsed;
                if (!jokerUsed)
                {
                    jokerUsed = AskedQuestions.FirstOrDefault(q => q.JokerUsed) != null;
                }

                return jokerUsed;
            }
        }

        public List<int> QuestionIds
        {
            get
            {
                var questionIds = new List<int>();

                if (CurrentQuestion != null)
                {
                    questionIds.Add(CurrentQuestion.QuizQuestion.QuestionId);
                }

                AskedQuestions.ForEach(q => questionIds.Add(q.QuizQuestion.QuestionId));

                return questionIds;
            }
        }

        public QuizQuestion UseJoker()
        {
            return CurrentQuestion.UseJoker();
        }
        
        public RunningGame(int userId, IEnumerable<int> categories)
        {
            UserId = userId;
            Categories = categories.ToList();
        }

        public void AskQuestion(Question question)
        {
            if (CurrentQuestion != null)
            {
                AskedQuestions.Add(CurrentQuestion);
            }

            question.TimeAsked = DateTime.Now;
            CurrentQuestion = question;
        }

        public void AnswerQuestion(Answer answer)
        {
            CurrentQuestion.TimeAnswered = DateTime.Now;
            CurrentQuestion.AnsweredAnswer = answer;
        }

        public QuizResult End(bool won = false)
        {
            int timeElapsed = 0;
            foreach (var question in AskedQuestions)
            {
                timeElapsed += (question.TimeAnswered - question.TimeAsked).Seconds;
            }

            return new QuizResult
            {
                Won = won,
                Points = AskedQuestions.Count * 30, // since only correctly answered questions land in the list
                TimeElapsed = timeElapsed,
                JokerUsed = JokerUsed
            };
        }
    }
}