using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhoWantsToBeAMillionaire.Models.Data.Quiz
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public List<QuizQuestion> Questions { get; set; } = new List<QuizQuestion>();

        public Category()
        {
        }

        public Category(int categoryId, string name)
        {
            CategoryId = categoryId;
            Name = name;
        }
    }
}