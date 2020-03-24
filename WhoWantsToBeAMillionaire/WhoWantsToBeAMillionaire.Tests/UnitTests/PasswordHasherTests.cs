using WhoWantsToBeAMillionaire.Models.Lifecycle.Users;
using Xunit;

namespace WhoWantsToBeAMillionaire.Tests.UnitTests
{
    public class PasswordHasherTests
    {
        [Fact]
        public void TestHashPassword()
        {
            var password = "user123";
            
            var orgHasher = new PasswordHasher();

            var salt = orgHasher.GenerateSalt().Salt;
            
            var orgResult = orgHasher.HashPassword(salt, password).Hashed;
            
            var newHasher = new PasswordHasher();
            var newResult = newHasher.HashPassword(salt, password).Hashed;
            
            Assert.Equal(orgResult, newResult);
        }
    }
}