using Microsoft.Extensions.Configuration;

namespace WhoWantsToBeAMillionaire.Tests
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