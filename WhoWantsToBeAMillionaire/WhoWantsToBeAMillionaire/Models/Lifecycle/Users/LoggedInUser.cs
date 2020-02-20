using System;

namespace WhoWantsToBeAMillionaire.Models.Lifecycle.Users
{
    public class LoggedInUser
    {
        private DateTime _lastRequest { get; set; }
        public int UserId { get; private set; }
        public string Token { get; private set; }

        public LoggedInUser(int userId, string token)
        {
            UserId = userId;
            Token = token;
            _lastRequest = DateTime.Now;
        }

        public void Request()
        {
            _lastRequest = DateTime.Now;
        }

        public bool ValidToken(string token)
        {
            return DateTime.Now.Subtract(_lastRequest).Minutes <= 60 && token == Token;
        }
    }
}