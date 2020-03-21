using System;

namespace WhoWantsToBeAMillionaire.Models.Exceptions
{
    public class NoQuestionsAnsweredException : Exception
    {
        public NoQuestionsAnsweredException(string message) : base(message)
        {
        }
    }
}