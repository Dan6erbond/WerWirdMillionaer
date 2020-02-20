using System;
using System.Collections.Generic;
using System.Linq;
using WhoWantsToBeAMillionaire.Models.Data.Users;
using WhoWantsToBeAMillionaire.Models.Exceptions;

namespace WhoWantsToBeAMillionaire.Models.Lifecycle.Users
{
    public class UserManager
    {
        private readonly IRepository<User> _userRepository;
        private readonly List<LoggedInUser> _loggedInUsers = new List<LoggedInUser>();

        public UserManager(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public void CreateUser(UserCredentials credentials)
        {
            var specification = new UserSpecification(username: credentials.Username);
            if (_userRepository.Query(specification).FirstOrDefault() != null)
            {
                throw new UserAlreadyExistsException($"User {credentials.Username} already exists.");
            }

            PasswordHasher hasher = new PasswordHasher();
            hasher.GenerateSalt().HashPassword(hasher.Salt, credentials.Password);

            var user = new User(credentials.Username, hasher.Salt, hasher.Hashed);

            _userRepository.Create(user);
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

            if (password != user.Password)
            {
                throw new IncorrectPasswordException($"Incorrect password given for user {credentials.Username}");
            }

            var token = Guid.NewGuid().ToString();
            
            var loggedInUser = new LoggedInUser(user.UserId, token);
            _loggedInUsers.Add(loggedInUser);
            
            return token;
        }

        public User GetUser(string token)
        {
            var loggedInUser =
                _loggedInUsers.FirstOrDefault(u => u.ValidToken(token));
            
            if (loggedInUser == null)
            {
                throw new InvalidTokenException($"Invalid or expired token given.");
            }
            
            var index = _loggedInUsers.FindIndex(u => u.UserId == loggedInUser.UserId);
            _loggedInUsers[index].Request();

            var specification = new UserSpecification(userId: loggedInUser.UserId);
            var user = _userRepository.Query(specification).First();
            return user;
        }
    }
}