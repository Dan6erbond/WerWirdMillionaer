using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Cms;
using WhoWantsToBeAMillionaire.Models.Data.Quiz;

namespace WhoWantsToBeAMillionaire.Models.Lifecycle.Games
{
    public class GameQuestion : IQuestion<GameAnswer>
    {
        public int QuestionId { get; set; }
        public int CategoryId { get; set; }
        public string Question { get; set; }
        public List<GameAnswer> Answers { get; set; } = new List<GameAnswer>();
        public int TimesAsked { get; set; }
        public int CorrectlyAnswered { get; set; }
        [JsonIgnore] public GameAnswer AnsweredAnswer { get; set; }
        [JsonIgnore] public bool JokerUsed { get; private set; }
        [JsonIgnore] public DateTime TimeAsked { get; set; }
        [JsonIgnore] public DateTime TimeAnswered { get; set; }

        public GameQuestion(QuizQuestion question, int timesAsked, int correctlyAnswered)
        {
            QuestionId = question.QuestionId;
            CategoryId = question.CategoryId;
            Question = question.Question;

            foreach (var answer in question.Answers)
            {
                Answers.Add(new GameAnswer(answer));
            }

            Answers.Shuffle();

            TimesAsked = timesAsked;
            CorrectlyAnswered = correctlyAnswered;
        }

        public GameQuestion UseJoker()
        {
            JokerUsed = true;

            var random = new Random();
            var indexes = new List<int>();
            for (int i = 0; i < 2; i++)
            {
                int index;
                do
                {
                    index = random.Next(Answers.Count);
                } while (Answers[index].Correct || indexes.Contains(index));

                indexes.Add(index);
                Answers[index].HiddenAnswer = true;
            }

            return this;
        }
    }
}