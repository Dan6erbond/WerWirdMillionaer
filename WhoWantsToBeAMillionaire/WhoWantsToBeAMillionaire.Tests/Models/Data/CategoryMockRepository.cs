using System.Collections.Generic;
using System.Linq;
using WhoWantsToBeAMillionaire.Models;
using WhoWantsToBeAMillionaire.Models.Data.Quiz;

namespace WhoWantsToBeAMillionaire.Tests.Models.Data
{
    public class CategoryMockRepository : IRepository<Category>
    {
        public IEnumerable<Category> List { get; private set; }

        public CategoryMockRepository()
        {
            List = new List<Category>
            {
                new Category(0, "Automotives")
            };
        }

        public int Create(Category item)
        {
            var id = List.Last().CategoryId + 1;
            item.CategoryId = id;
            
            var newList = List.ToList();
            newList.Add(item);
            
            List = newList;
            
            return id;
        }

        public void Update(Category item)
        {
            var newList = List.ToList();
            var index = newList.FindIndex(i => i.CategoryId == item.CategoryId);
            newList[index] = item;
            
            List = newList;
        }

        public void Delete(Category item)
        {
            var newList = List.ToList();
            var index = newList.FindIndex(i => i.CategoryId == item.CategoryId);
            newList.RemoveAt(index);
            
            List = newList;
        }

        public List<Category> Query(ISpecification<Category> specification)
        {
            return List.Where(specification.Specificied).ToList();
        }
    }
}