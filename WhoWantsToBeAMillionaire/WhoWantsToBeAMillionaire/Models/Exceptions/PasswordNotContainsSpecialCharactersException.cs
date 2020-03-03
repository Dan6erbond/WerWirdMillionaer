using System;

namespace WhoWantsToBeAMillionaire.Models.Exceptions
{
    public class PasswordNotContainsSpecialCharactersException : Exception
    {
        public PasswordNotContainsSpecialCharactersException(string message) : base(message)
        {
        }
    }
}