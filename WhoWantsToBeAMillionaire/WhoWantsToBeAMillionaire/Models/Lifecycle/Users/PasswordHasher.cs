using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace WhoWantsToBeAMillionaire.Models.Lifecycle.Users
{
    public class PasswordHasher
    {
        public byte[] Salt { get; set; }
        public string Hashed { get; set; }

        public PasswordHasher()
        {
        }

        public PasswordHasher(string salt)
        {
            Salt = Convert.FromBase64String(salt);
        }

        public PasswordHasher GenerateSalt()
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            Salt = salt;

            return this;
        }

        public PasswordHasher HashPassword(byte[] salt, string password)
        {
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
            Hashed = hashed;

            return this;
        }
    }
}