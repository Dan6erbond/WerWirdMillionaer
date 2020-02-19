namespace WhoWantsToBeAMillionaire.Models.Api.ApiErrors.Users
{
    public class IncorrectPasswordError : ApiError
    {
        public IncorrectPasswordError() : base(400, "Incorrect password given.")
        {
        }

        public IncorrectPasswordError(string message) : base(400, "Incorrect password given.", message)
        {
        }
    }
}