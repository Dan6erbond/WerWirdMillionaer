using System;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using WhoWantsToBeAMillionaire.AutomatedUiTests.Models.Data;
using WhoWantsToBeAMillionaire.Models;
using WhoWantsToBeAMillionaire.Models.Data.Games;
using WhoWantsToBeAMillionaire.Models.Data.Quiz;
using WhoWantsToBeAMillionaire.Models.Data.Users;
using WhoWantsToBeAMillionaire.Models.Lifecycle.Admin;
using WhoWantsToBeAMillionaire.Models.Lifecycle.Games;
using WhoWantsToBeAMillionaire.Models.Lifecycle.Users;

namespace WhoWantsToBeAMillionaire.AutomatedUiTests
{
    public class CustomWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup : class
    {
        private readonly IConfiguration _configuration;

        public CustomWebApplicationFactory()
        {
            _configuration = TestHelper.InitConfiguration();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddControllers().AddNewtonsoftJson();
                services.AddControllersWithViews();

                services.AddMvc()
                    .AddJsonOptions(options => { options.JsonSerializerOptions.IgnoreNullValues = true; });

                // In production, the React files will be served from this directory
                services.AddSpaStaticFiles(configuration => { configuration.RootPath = "ClientApp/build"; });

                services.AddSingleton<IRepository<User>, UserMockRepository>();
                services.AddSingleton<IRepository<Category>, CategoryMockRepository>();
                services.AddSingleton<IRepository<QuizQuestion>, QuizQuestionMockRepository>();
                services.AddSingleton<IRepository<QuizAnswer>, QuizAnswerMockRepository>();
                services.AddSingleton<IRepository<Round>, RoundMockRepository>();
                services.AddSingleton<IRepository<Game>, GameMockRepository>();
                services.AddSingleton<IRepository<CategoryGame>, CategoryGameMockRepository>();

                services.AddSingleton<UserManager>();
                services.AddSingleton<GameManager>();
                services.AddSingleton<DataManager>();

                services.AddAuthentication(options =>
                    {
                        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    })
                    .AddJwtBearer(options =>
                    {
                        options.SaveToken = true;
                        options.RequireHttpsMetadata = false;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidAudience = _configuration["Tokens:Audience"],
                            ValidIssuer = _configuration["Tokens:Issuer"],
                            IssuerSigningKey =
                                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:SecureKey"]))
                        };
                    });

                services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            });
        }
    }
}