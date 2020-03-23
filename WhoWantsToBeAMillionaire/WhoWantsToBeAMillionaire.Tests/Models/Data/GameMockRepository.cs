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
    public class GameMockRepository : IRepository<Game>
    {
        public IEnumerable<Game> List { get; private set; }

        public GameMockRepository()
        {
            List = new List<Game>
            {
                new Game(0, 1, DateTime.Now)
            };
        }

        public int Create(Game item)
        {
            var id = List.Last().GameId + 1;
            item.GameId = id;
            
            var newList = List.ToList();
            newList.Add(item);
            
            List = newList;
            
            return id;
        }

        public void Update(Game item)
        {
            var newList = List.ToList();
            var index = newList.FindIndex(i => i.GameId == item.GameId);
            newList[index] = item;
            
            List = newList;
        }

        public void Delete(Game item)
        {
            var newList = List.ToList();
            var index = newList.FindIndex(i => i.GameId == item.GameId);
            newList.RemoveAt(index);
            
            List = newList;
        }

        public List<Game> Query(ISpecification<Game> specification)
        {
            return List.Where(specification.Specificied).ToList();
        }
    }
}