using System;
using System.Collections.Generic;
using System.Linq;
using WhoWantsToBeAMillionaire.Models;
using WhoWantsToBeAMillionaire.Models.Data.Games;
using WhoWantsToBeAMillionaire.Models.Data.Quiz;
using WhoWantsToBeAMillionaire.Models.Data.Users;
using WhoWantsToBeAMillionaire.Models.Lifecycle.Users;

namespace WhoWantsToBeAMillionaire.AutomatedUiTests.Models.Data
{
    public class RoundMockRepository : IRepository<Round>
    {
        public IEnumerable<Round> List { get; private set; }

        public RoundMockRepository()
        {
            List = new List<Round>
            {
                new Round(0, 0, 0, 5, 0)
            };
        }

        public int Create(Round item)
        {
            var id = List.Last().RoundId + 1;
            item.RoundId = id;
            
            var newList = List.ToList();
            newList.Add(item);
            
            List = newList;
            
            return id;
        }

        public void Update(Round item)
        {
            var newList = List.ToList();
            var index = newList.FindIndex(i => i.RoundId == item.RoundId);
            newList[index] = item;
            
            List = newList;
        }

        public void Delete(Round item)
        {
            var newList = List.ToList();
            var index = newList.FindIndex(i => i.RoundId == item.RoundId);
            newList.RemoveAt(index);
            
            List = newList;
        }

        public List<Round> Query(ISpecification<Round> specification)
        {
            return List.Where(specification.Specificied).ToList();
        }
    }
}