namespace WhoWantsToBeAMillionaire.Models.Api.ApiErrors.Users
{
    public class PasswordNotContainsSpecialCharactersError : ApiError
    {
        public PasswordNotContainsSpecialCharactersError(string message) : base(
            "PASSWORD_NOT_CONTAINS_SPECIAL_CHARACTERS", 400, "Incorrect password given.", message)
        {
        }
    }
}