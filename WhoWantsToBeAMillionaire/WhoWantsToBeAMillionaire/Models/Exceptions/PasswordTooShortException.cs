using System;

namespace WhoWantsToBeAMillionaire.Models.Exceptions
{
    public class PasswordTooShortException : Exception
    {
        public PasswordTooShortException(string message) : base(message)
        {
        }
    }
}