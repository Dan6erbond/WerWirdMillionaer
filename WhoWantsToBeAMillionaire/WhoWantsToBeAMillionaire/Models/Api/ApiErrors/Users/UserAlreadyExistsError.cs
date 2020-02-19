namespace WhoWantsToBeAMillionaire.Models.Api.ApiErrors.Users
{
    public class UserAlreadyExistsError : ApiError
    {
        public UserAlreadyExistsError() : base(400, "User already exists.")
        {
        }

        public UserAlreadyExistsError(string message) : base(400, "User already exists.", message)
        {
        }
    }
}