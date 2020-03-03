using System;

namespace WhoWantsToBeAMillionaire.Models.Exceptions
{
    public class PasswordNotContainsLettersException : Exception
    {
        public PasswordNotContainsLettersException(string message) : base(message)
        {
        }
    }
}