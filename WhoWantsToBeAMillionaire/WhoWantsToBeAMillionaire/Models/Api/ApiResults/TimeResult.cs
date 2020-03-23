namespace WhoWantsToBeAMillionaire.Models.Api.ApiResults
{
    public class TimeResult
    {
        public string Type { get; } = "TIME_RESULT";
        public int GameTime { get; }
        public int QuestionTime { get; }

        public TimeResult(int gameTime, int questionTime)
        {
            GameTime = gameTime;
            QuestionTime = questionTime;
        }
    }
}