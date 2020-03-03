using Newtonsoft.Json;

namespace WhoWantsToBeAMillionaire.Models.Api.ApiErrors
{
    public class ApiError
    {
        public string Type { get; private set; }
        public int Status { get; private set; }
        public string Title { get; private set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Message { get; private set; }

        public ApiError(string type, int statusCode, string statusDescription)
        {
            this.Type = type;
            this.Status = statusCode;
            this.Title = statusDescription;
        }

        public ApiError(string type, int statusCode, string statusDescription, string message)
            : this(type, statusCode, statusDescription)
        {
            this.Message = message;
        }
    }
}