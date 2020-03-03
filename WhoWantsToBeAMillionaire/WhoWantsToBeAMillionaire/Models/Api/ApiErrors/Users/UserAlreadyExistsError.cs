namespace WhoWantsToBeAMillionaire.Models.Api.ApiErrors.Users
{
    public class UserAlreadyExistsError : ApiError
    {
        public UserAlreadyExistsError(string message) : base("USER_ALREADY_EXISTS", 400, "User already exists.",
            message)
        {
        }
    }
}