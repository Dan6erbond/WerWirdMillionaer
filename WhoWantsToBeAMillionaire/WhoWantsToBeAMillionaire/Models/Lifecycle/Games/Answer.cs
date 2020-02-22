﻿namespace WhoWantsToBeAMillionaire.Models.Lifecycle.Games
{
    public class Answer
    {
        public int AnswerId { get; }
        public bool Correct { get; }

        public Answer(int answerId, bool correct)
        {
            AnswerId = answerId;
            Correct = correct;
        }
    }
}