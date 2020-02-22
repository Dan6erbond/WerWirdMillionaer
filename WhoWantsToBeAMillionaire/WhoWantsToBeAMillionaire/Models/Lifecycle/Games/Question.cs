﻿namespace WhoWantsToBeAMillionaire.Models.Lifecycle.Games
{
    public class Question
    {
        public int QuestionId { get; }
        public Answer AnsweredAnswer { get; set; }

        public Question(int questionId)
        {
            QuestionId = questionId;
        }
    }
}