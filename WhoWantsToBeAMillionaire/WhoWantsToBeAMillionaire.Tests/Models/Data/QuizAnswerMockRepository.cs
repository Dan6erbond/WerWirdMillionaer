using System.Collections.Generic;
using System.Linq;
using WhoWantsToBeAMillionaire.Models;
using WhoWantsToBeAMillionaire.Models.Data.Quiz;
using WhoWantsToBeAMillionaire.Models.Data.Users;
using WhoWantsToBeAMillionaire.Models.Lifecycle.Users;

namespace WhoWantsToBeAMillionaire.AutomatedUiTests.Models.Data
{
    public class QuizAnswerMockRepository : IRepository<QuizAnswer>
    {
        public IEnumerable<QuizAnswer> List { get; private set; }

        public QuizAnswerMockRepository()
        {
            List = new List<QuizAnswer>
            {
                new QuizAnswer(0, 0, "Enzo Ferrari"),
                new QuizAnswer(0, 0, "Marco Ferrari"),
                new QuizAnswer(0, 0, "Ferrrari Murcielago"),
                new QuizAnswer(0, 0, "Ferruccio Ferrari")
            };
        }

        public int Create(QuizAnswer item)
        {
            var id = List.Last().AnswerId + 1;
            item.AnswerId = id;
            
            var newList = List.ToList();
            newList.Add(item);
            
            List = newList;
            
            return id;
        }

        public void Update(QuizAnswer item)
        {
            var newList = List.ToList();
            var index = newList.FindIndex(i => i.AnswerId == item.AnswerId);
            newList[index] = item;
            
            List = newList;
        }

        public void Delete(QuizAnswer item)
        {
            var newList = List.ToList();
            var index = newList.FindIndex(i => i.AnswerId == item.AnswerId);
            newList.RemoveAt(index);
            
            List = newList;
        }

        public List<QuizAnswer> Query(ISpecification<QuizAnswer> specification)
        {
            return List.Where(specification.Specificied).ToList();
        }
    }
}