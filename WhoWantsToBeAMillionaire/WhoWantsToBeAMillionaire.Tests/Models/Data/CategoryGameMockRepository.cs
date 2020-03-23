using System.Collections.Generic;
using System.Linq;
using WhoWantsToBeAMillionaire.Models;
using WhoWantsToBeAMillionaire.Models.Data.Games;

namespace WhoWantsToBeAMillionaire.Tests.Models.Data
{
    public class CategoryGameMockRepository : IRepository<CategoryGame>
    {
        public IEnumerable<CategoryGame> List { get; private set; }

        public CategoryGameMockRepository()
        {
            List = new List<CategoryGame>
            {
                new CategoryGame(0, 0, 0)
            };
        }

        public int Create(CategoryGame item)
        {
            var id = List.Last().CategoryGameId + 1;
            item.CategoryGameId = id;
            
            var newList = List.ToList();
            newList.Add(item);
            
            List = newList;
            
            return id;
        }

        public void Update(CategoryGame item)
        {
            var newList = List.ToList();
            var index = newList.FindIndex(i => i.CategoryGameId == item.CategoryGameId);
            newList[index] = item;
            
            List = newList;
        }

        public void Delete(CategoryGame item)
        {
            var newList = List.ToList();
            var index = newList.FindIndex(i => i.CategoryGameId == item.CategoryGameId);
            newList.RemoveAt(index);
            
            List = newList;
        }

        public List<CategoryGame> Query(ISpecification<CategoryGame> specification)
        {
            return List.Where(specification.Specificied).ToList();
        }
    }
}