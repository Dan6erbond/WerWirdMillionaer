namespace WhoWantsToBeAMillionaire.Models.Data.Quiz
{
    public class AnswerResult
    {
        public string Type { get; } = "ANSWER_RESULT";
        public readonly bool Correct;

        public AnswerResult(bool correct)
        {
            Correct = correct;
        }
    }
}