namespace WhoWantsToBeAMillionaire.Models.Data.Users
{
    public class UserSpecification : ISpecification<User>
    {
        private readonly int? _userId = null;
        private readonly string _username = null;

        public UserSpecification(int? userId = null, string username = null)
        {
            _userId = userId;
            _username = username;
        }
        
        public bool Specificied(User item)
        {
            if (_userId != null && item.UserId != _userId)
            {
                return false;
            }

            if (_username != null && item.Username != _username)
            {
                return false;
            }

            return true;
        }
    }
}