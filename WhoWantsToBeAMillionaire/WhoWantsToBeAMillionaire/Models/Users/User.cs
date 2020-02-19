using System;

namespace WhoWantsToBeAMillionaire.Models.Users
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public bool IsAdmin { get; set; } = false;
        public string Salt { get; set; }
        public string Password { get; set; }

        public User()
        {
        }

        public User(string username, byte[] salt, string password)
        {
            Username = username;
            Salt = Convert.ToBase64String(salt);
            Password = password;
        }
    }
}