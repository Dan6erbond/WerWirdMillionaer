using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WhoWantsToBeAMillionaire.Models;
using WhoWantsToBeAMillionaire.Models.Data;
using WhoWantsToBeAMillionaire.Models.Data.Quiz;
using WhoWantsToBeAMillionaire.Models.Data.Users;

namespace WhoWantsToBeAMillionaire
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            InitializeDbIfNotInitialized(host);

            host.Run();
        }

        private static void InitializeDbIfNotInitialized(IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                var userRepository = services.GetRequiredService<IRepository<User>>();
                var categoryRepository = services.GetRequiredService<IRepository<Category>>();
                var questionRepository = services.GetRequiredService<IRepository<QuizQuestion>>();
                var answerRepository = services.GetRequiredService<IRepository<QuizAnswer>>();
                DbInitializer.Initialize(userRepository, categoryRepository, questionRepository, answerRepository);
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred creating the DB.");
            }
        }
        
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}