﻿using Newtonsoft.Json;
 using WhoWantsToBeAMillionaire.Models.Data.Quiz;

 namespace WhoWantsToBeAMillionaire.Models.Lifecycle.Games
{
    public class GameAnswer : IAnswer
    {
        public int AnswerId { get; }
        [JsonIgnore] public bool Correct { get; }
        [JsonIgnore] public bool HiddenAnswer { get; set; } = false;
        public string Answer { get; set; }

        public GameAnswer(QuizAnswer answer)
        {
            AnswerId = answer.AnswerId;
            Correct = answer.Correct;
            Answer = answer.Answer;
        }

        // expose Correct property to API so false answers can be hidden when Joker is used
        public bool ShouldSerializeCorrect() => HiddenAnswer;
    }
}