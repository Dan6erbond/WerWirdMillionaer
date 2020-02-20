using System;

namespace WhoWantsToBeAMillionaire.Models.Exceptions
{
    public class InvalidTokenException : Exception
    {
        public InvalidTokenException(string message) : base(message)
        {
        }
    }
}