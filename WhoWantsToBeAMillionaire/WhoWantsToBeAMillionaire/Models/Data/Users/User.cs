﻿using System;
using System.Text.RegularExpressions;
using WhoWantsToBeAMillionaire.Models.Exceptions;

namespace WhoWantsToBeAMillionaire.Models.Data.Users
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

        public static bool PasswordIsValid(string password)
        {
            if (password.Length <= 5)
            {
                throw new PasswordTooShortException("Passwords must be 6 characters or greater in length.");
            }

            var letterRx = new Regex(@"[a-zA-Z]");
            if (!letterRx.IsMatch(password))
            {
                throw new PasswordNotContainsLettersException("Passwords must contain at least one upper- or lowercase letter.");
            }

            var numberRx = new Regex(@"[0-9]");
            var symbolRx = new Regex(@"[!@#\$%\^&\*]");
            if (!numberRx.IsMatch(password) && !symbolRx.IsMatch(password))
            {
                throw new PasswordNotContainsSpecialCharactersException("Passwords must contain at least one number or special character.");
            }

            return true;
        }
    }
}