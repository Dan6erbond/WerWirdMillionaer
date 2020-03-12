﻿﻿using Newtonsoft.Json;
 using WhoWantsToBeAMillionaire.Models.Data.Quiz;

 namespace WhoWantsToBeAMillionaire.Models.Lifecycle.Games
{
    public class GameAnswer : IAnswer
    {
        public int AnswerId { get; }
        public bool Correct { get; }
         public bool HiddenAnswer { get; set; }
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