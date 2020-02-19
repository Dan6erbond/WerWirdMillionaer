namespace WhoWantsToBeAMillionaire.Models.Api.ApiErrors.Users
{
    public class UserDoesNotExistError : ApiError
    {
        public UserDoesNotExistError() : base(400, "User does not exist.")
        {
        }

        public UserDoesNotExistError(string message) : base(400, "User does not exist.", message)
        {
        }
    }
}