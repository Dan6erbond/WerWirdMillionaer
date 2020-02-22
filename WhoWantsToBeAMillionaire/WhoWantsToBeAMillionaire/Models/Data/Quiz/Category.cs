using System.Collections.Generic;

namespace WhoWantsToBeAMillionaire.Models.Data.Quiz
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public List<QuizQuestion> Questions { get; set; } = new List<QuizQuestion>();
    }
}