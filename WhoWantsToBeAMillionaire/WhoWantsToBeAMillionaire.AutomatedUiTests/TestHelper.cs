using Microsoft.Extensions.Configuration;

namespace WhoWantsToBeAMillionaire.AutomatedUiTests
{
    public class TestHelper
    {
        public static IConfiguration InitConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            return config;
        }
    }
}