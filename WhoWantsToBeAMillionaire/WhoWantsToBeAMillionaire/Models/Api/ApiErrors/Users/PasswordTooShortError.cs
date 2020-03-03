namespace WhoWantsToBeAMillionaire.Models.Api.ApiErrors.Users
{
    public class PasswordTooShortError : ApiError
    {
        public PasswordTooShortError(string message) : base("PASSWORD_TOO_SHORT", 400, "Incorrect password given.", message)
        {
        }
    }
}