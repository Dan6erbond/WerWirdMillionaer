namespace WhoWantsToBeAMillionaire.Models.Api.ApiErrors.Users
{
    public class IncorrectPasswordError : ApiError
    {
        public IncorrectPasswordError(string message) : base("INCORRECT_PASSWORD", 400, "Incorrect password given.", message)
        {
        }
    }
}