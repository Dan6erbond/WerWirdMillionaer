namespace WhoWantsToBeAMillionaire.Models.Api.ApiErrors.Users
{
    public class NoQuestionsAnsweredError : ApiError
    {
        public NoQuestionsAnsweredError(string message) : base("NO_QUESTIONS_ANSWERED", 400, "No questions have been answered.", message)
        {
        }
    }
}