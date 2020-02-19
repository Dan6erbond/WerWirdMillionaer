using System;
using System.Collections.Generic;
using System.Linq;
using WhoWantsToBeAMillionaire.Models.Exceptions;

namespace WhoWantsToBeAMillionaire.Models.Users
{
    public class UserManager
    {
        private readonly IRepository<User> _userRepository;
        private readonly List<User> _loggedInUsers = new List<User>();

        public UserManager(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public string LogInUser(UserCredentials credentials)
        {
            var specification = new UserSpecification(username: credentials.Username);
            var user = _userRepository.Query(specification).FirstOrDefault();

            if (user == null)
            {
                throw new UserDoesNotExistException($"User {credentials.Username} does not exist.");
            }
            
            var hasher = new PasswordHasher(user.Salt);
            var password = hasher.HashPassword(hasher.Salt, credentials.Password).Hashed;
            
            if (password == user.Password)
            {
                var token = Guid.NewGuid().ToString();
                user.Token = token;
                _loggedInUsers.Add(user);
                return token;
            }

            throw new IncorrectPasswordException($"Incorrect password given for user {credentials.Username}");
        }
        
    }
}