namespace WhoWantsToBeAMillionaire.Models.Data.Quiz
{
    public class AnswerResult
    {
        public string Type { get; } = "ANSWER_RESULT";
        public readonly bool Correct;
        public readonly int QuestionDuration;

        public AnswerResult(bool correct, int duration)
        {
            Correct = correct;
            QuestionDuration = duration;
        }
    }
}