using System.Collections.Generic;

namespace WhoWantsToBeAMillionaire.Models
{
    public interface IQuestion<T>
    {
        public int QuestionId { get; set; }
        public string Question { get; set; }
        public List<T> Answers { get; set; }
    }
}