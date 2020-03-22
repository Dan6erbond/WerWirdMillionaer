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
        public List<GameQuestion> AskedQuestions { get; } = new List<GameQuestion>();
        public GameQuestion CurrentQuestion { get; private set; }
        public DateTime TimeStarted { get; }

        public bool JokerUsed
        {
            get
            {
                var jokerUsed = CurrentQuestion?.JokerUsed ?? false;
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
                if (CurrentQuestion != null) questionIds.Add(CurrentQuestion.QuestionId);
                AskedQuestions.ForEach(q => questionIds.Add(q.QuestionId));
                return questionIds;
            }
        }

        public GameQuestion UseJoker()
        {
            return CurrentQuestion.UseJoker();
        }

        public RunningGame(int userId, IEnumerable<int> categories)
        {
            TimeStarted = DateTime.Now;
            UserId = userId;
            Categories = categories.ToList();
        }

        public void AskQuestion(GameQuestion gameQuestion)
        {
            gameQuestion.TimeAsked = DateTime.Now;
            CurrentQuestion = gameQuestion;
        }

        public AnswerResult AnswerQuestion(QuizAnswer answer)
        {
            CurrentQuestion.TimeAnswered = DateTime.Now;
            CurrentQuestion.AnsweredAnswer = CurrentQuestion.Answers.First(a => a.AnswerId == answer.AnswerId);
            var duration = (int) (CurrentQuestion.TimeAnswered - CurrentQuestion.TimeAsked).TotalSeconds;

            AskedQuestions.Add(CurrentQuestion);
            CurrentQuestion = null;

            return new AnswerResult(answer.Correct, duration);
        }

        public QuizResult End(bool won, bool timeOver)
        {
            var timeEnded = DateTime.Now;
            int timeElapsed = (int) (timeEnded - TimeStarted).TotalSeconds;

            int points = 0;
            if (won) points = AskedQuestions.Count * 30;

            return new QuizResult
            {
                Won = won,
                Points = points,
                TimeElapsed = timeElapsed,
                JokerUsed = JokerUsed,
                TimeOver = timeOver
            };
        }
    }
}