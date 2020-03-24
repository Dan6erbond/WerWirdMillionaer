using System;
using WhoWantsToBeAMillionaire.Models.Data.Users;
using WhoWantsToBeAMillionaire.Models.Exceptions;
using Xunit;

namespace WhoWantsToBeAMillionaire.Tests.UnitTests
{
    public class UserTests
    {
        [Fact]
        public void TestPasswordIsValid()
        {
            var unsafePassword = "user";
            var safePassword = "user123";

            try
            {
                Assert.True(!User.PasswordIsValid(unsafePassword));
            }
            catch (Exception e)
            {
                Assert.IsType<PasswordTooShortException>(e);
            }

            Assert.True(User.PasswordIsValid(safePassword));
        }
    }
}