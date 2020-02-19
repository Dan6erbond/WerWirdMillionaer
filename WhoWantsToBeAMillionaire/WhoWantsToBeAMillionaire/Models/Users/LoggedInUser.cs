namespace WhoWantsToBeAMillionaire.Models.Users
{
    public class LoggedInUser
    {
        public int UserId { get; private set; }
        public string Token { get; private set; }

        public LoggedInUser(int userId, string token)
        {
            UserId = userId;
            Token = token;
        }
    }
}