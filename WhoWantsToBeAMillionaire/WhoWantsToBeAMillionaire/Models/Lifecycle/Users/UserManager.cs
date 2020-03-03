using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WhoWantsToBeAMillionaire.Models.Data.Users;
using WhoWantsToBeAMillionaire.Models.Exceptions;

namespace WhoWantsToBeAMillionaire.Models.Lifecycle.Users
{
    public class UserManager
    {
        private readonly IRepository<User> _userRepository;
        private readonly IConfiguration _configuration;
        
        public UserManager(IRepository<User> userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public void CreateUser(UserCredentials credentials)
        {
            var specification = new UserSpecification(username: credentials.Username);
            if (_userRepository.Query(specification).FirstOrDefault() != null)
            {
                throw new UserAlreadyExistsException($"User {credentials.Username} already exists.");
            }

            User.PasswordIsValid(credentials.Password);

            PasswordHasher hasher = new PasswordHasher();
            hasher.GenerateSalt().HashPassword(hasher.Salt, credentials.Password);

            var user = new User(credentials.Username, hasher.Salt, hasher.Hashed);

            _userRepository.Create(user);
        }

        public JwtSecurityToken LoginUser(UserCredentials credentials)
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
            
            var authClaims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:SecureKey"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["Tokens:Issuer"],
                audience: _configuration["Tokens:Audience"],
                expires: DateTime.Now.AddHours(8),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );
            
            return token;
        }

        public User GetUser(string username)
        {
            var specification = new UserSpecification(username: username);
            var user = _userRepository.Query(specification).First();
            return user;
        }
    }
}