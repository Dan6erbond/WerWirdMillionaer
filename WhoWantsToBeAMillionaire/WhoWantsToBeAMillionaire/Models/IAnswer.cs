namespace WhoWantsToBeAMillionaire.Models
{
    public interface IAnswer
    {
        public int AnswerId { get; }
        public bool Correct { get; }
        public string Answer { get; set; }
    }
}