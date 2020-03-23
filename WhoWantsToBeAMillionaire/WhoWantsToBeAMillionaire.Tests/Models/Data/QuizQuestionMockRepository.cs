using System.Collections.Generic;
using System.Linq;
using WhoWantsToBeAMillionaire.Models;
using WhoWantsToBeAMillionaire.Models.Data.Quiz;

namespace WhoWantsToBeAMillionaire.Tests.Models.Data
{
    public class QuizQuestionMockRepository : IRepository<QuizQuestion>
    {
        public IEnumerable<QuizQuestion> List { get; private set; }

        public QuizQuestionMockRepository()
        {
            List = new List<QuizQuestion>
            {
                new QuizQuestion(0, 0, "Who invented Ferrari?")
            };
        }

        public int Create(QuizQuestion item)
        {
            var id = List.Last().QuestionId + 1;
            item.QuestionId = id;
            
            var newList = List.ToList();
            newList.Add(item);
            
            List = newList;
            
            return id;
        }

        public void Update(QuizQuestion item)
        {
            var newList = List.ToList();
            var index = newList.FindIndex(i => i.QuestionId == item.QuestionId);
            newList[index] = item;
            
            List = newList;
        }

        public void Delete(QuizQuestion item)
        {
            var newList = List.ToList();
            var index = newList.FindIndex(i => i.QuestionId == item.QuestionId);
            newList.RemoveAt(index);
            
            List = newList;
        }

        public List<QuizQuestion> Query(ISpecification<QuizQuestion> specification)
        {
            return List.Where(specification.Specificied).ToList();
        }
    }
}