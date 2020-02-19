using System;

namespace WhoWantsToBeAMillionaire.Models.Exceptions
{
    public class IncorrectPasswordException : Exception
    {
        public IncorrectPasswordException(string message) : base(message)
        {
        }
    }
}