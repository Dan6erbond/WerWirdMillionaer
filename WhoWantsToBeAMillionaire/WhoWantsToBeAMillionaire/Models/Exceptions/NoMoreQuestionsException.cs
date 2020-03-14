using System;

namespace WhoWantsToBeAMillionaire.Models.Exceptions
{
    public class NoMoreQuestionsException : Exception
    {
        public NoMoreQuestionsException(string message) : base(message)
        {
        }
    }
}