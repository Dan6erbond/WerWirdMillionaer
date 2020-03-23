using System.Collections.Generic;
using System.Linq;
using WhoWantsToBeAMillionaire.Models;
using WhoWantsToBeAMillionaire.Models.Data.Users;
using WhoWantsToBeAMillionaire.Models.Lifecycle.Users;

namespace WhoWantsToBeAMillionaire.Tests.Models.Data
{
    public class UserMockRepository : IRepository<User>
    {
        public IEnumerable<User> List { get; private set; }

        public UserMockRepository()
        {
            var hasher = new PasswordHasher();

            hasher.GenerateSalt().HashPassword(hasher.Salt, "admin123");
            var admin = new User(0, "TestAdmin", hasher.Salt, hasher.Hashed, true);

            hasher.GenerateSalt().HashPassword(hasher.Salt, "user123");
            var user = new User(1, "TestUser", hasher.Salt, hasher.Hashed);

            List = new List<User> {admin, user};
        }

        public int Create(User item)
        {
            var id = List.Last().UserId + 1;
            item.UserId = id;

            var newList = List.ToList();
            newList.Add(item);

            List = newList;

            return id;
        }

        public void Update(User item)
        {
            var newList = List.ToList();
            var index = newList.FindIndex(i => i.UserId == item.UserId);
            newList[index] = item;

            List = newList;
        }

        public void Delete(User item)
        {
            var newList = List.ToList();
            var index = newList.FindIndex(i => i.UserId == item.UserId);
            newList.RemoveAt(index);

            List = newList;
        }

        public List<User> Query(ISpecification<User> specification)
        {
            return List.Where(specification.Specificied).ToList();
        }
    }
}