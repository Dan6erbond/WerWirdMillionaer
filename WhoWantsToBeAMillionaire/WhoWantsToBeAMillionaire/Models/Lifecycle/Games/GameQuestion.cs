using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using WhoWantsToBeAMillionaire.Models.Data.Quiz;

namespace WhoWantsToBeAMillionaire.Models.Lifecycle.Games
{
    public class GameQuestion : IQuestion<GameAnswer>
    {
        public int QuestionId { get; set; }
        public string Question { get; set; }
        public List<GameAnswer> Answers { get; set; } = new List<GameAnswer>();
        [JsonIgnore] public GameAnswer AnsweredAnswer { get; set; }
        [JsonIgnore] public bool JokerUsed { get; private set; } = false;
        [JsonIgnore] public DateTime TimeAsked { get; set; }
        [JsonIgnore] public DateTime TimeAnswered { get; set; }

        public GameQuestion(QuizQuestion question)
        {
            QuestionId = question.QuestionId;
            Question = question.Question;
            
            foreach (var answer in question.Answers)
            {
                Answers.Add(new GameAnswer(answer));
            }
            Answers.Shuffle();
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