namespace WhoWantsToBeAMillionaire.Models.Api.ApiErrors.Games
{
    public class NoMoreQuestionsError : ApiError
    {
        public NoMoreQuestionsError(string message) : base("NO_MORE_QUESTIONS", 400, "No more questions available.", message)
        {
        }
    }
}