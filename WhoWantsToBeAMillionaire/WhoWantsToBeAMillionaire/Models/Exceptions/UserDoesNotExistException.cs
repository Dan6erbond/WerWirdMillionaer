using System;

namespace WhoWantsToBeAMillionaire.Models.Exceptions
{
    public class UserDoesNotExistException : Exception
    {
        public UserDoesNotExistException(string message) : base(message)
        {
        }
    }
}