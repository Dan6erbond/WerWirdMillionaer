namespace WhoWantsToBeAMillionaire.Models.Api.ApiErrors.Users
{
    public class PasswordNotContainsLettersError : ApiError
    {
        public PasswordNotContainsLettersError(string message) : base("PASSWORD_NOT_CONTAINS_LETTERS", 400, "Incorrect password given.", message)
        {
        }
    }
}