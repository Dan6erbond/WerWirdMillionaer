namespace WhoWantsToBeAMillionaire.Models.Api.ApiErrors.Users
{
    public class InvalidTokenError : ApiError
    {
        public InvalidTokenError() : base(400, "Invalid token given.")
        {
        }

        public InvalidTokenError(string message) : base(400, "Invalid token given.", message)
        {
        }
    }
}