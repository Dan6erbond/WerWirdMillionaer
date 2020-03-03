namespace WhoWantsToBeAMillionaire.Models.Api.ApiErrors.Users
{
    public class UserDoesNotExistError : ApiError
    {
        public UserDoesNotExistError(string message) : base("USER_DOES_NOT_EXIST", 400, "User does not exist.", message)
        {
        }
    }
}