using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WhoWantsToBeAMillionaire.Tests.SeleniumTests;
using Xunit;
using Xunit.Abstractions;

namespace WhoWantsToBeAMillionaire.SeleniumTests
{
    public class AutomatedUiTests : IDisposable
    {
        private const int msTimeout = 150;

        private readonly IWebDriver _driver;
        private readonly ITestOutputHelper _outputHelper;

        private readonly LoginPage _loginPage;
        private readonly QuizPage _quizPage;

        public AutomatedUiTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
            _driver = new ChromeDriver();

            _loginPage = new LoginPage(_driver);
            _quizPage = new QuizPage(_driver);
        }

        [Fact]
        public void TestSiteLoad()
        {
            _loginPage.Navigate();

            Assert.Equal("WhoWantsToBeAMillionaire", _loginPage.Title);
            Assert.Contains("Log in", _loginPage.Source);
        }

        [Fact]
        public void TestLogin()
        {
            _loginPage.Navigate();

            _loginPage.PopulateUsername("TestUser");
            _loginPage.PopulatePassword("user123");
            _loginPage.ClickLogin();

            Thread.Sleep(msTimeout);

            Assert.Contains("quiz", _loginPage.Url);
        }

        [Fact]
        public void TestQuiz()
        {
            _loginPage.Navigate();

            _loginPage.PopulateUsername("TestUser");
            _loginPage.PopulatePassword("user123");
            _loginPage.ClickLogin();

            Thread.Sleep(msTimeout);

            var cat = "Automotives";

            _quizPage.SelectCategory(cat);
            _quizPage.ClickPlayButton();

            Assert.Equal(cat, _quizPage.QuestionCategoryBadge.Text);
        }

        [Fact]
        public void TestAnswerQuestion()
        {
            _loginPage.Navigate();

            _loginPage.PopulateUsername("TestUser");
            _loginPage.PopulatePassword("user123");
            _loginPage.ClickLogin();

            Thread.Sleep(msTimeout);

            var cat = "Automotives";

            _quizPage.SelectCategory(cat);
            _quizPage.ClickPlayButton();

            Thread.Sleep(msTimeout);

            var question = _quizPage.QuestionText.Text.Substring(0, _quizPage.QuestionText.Text.LastIndexOf(_quizPage.QuestionCategoryBadge.Text, StringComparison.Ordinal)).Trim();
            _outputHelper.WriteLine(question);

            Thread.Sleep(msTimeout);

            var rand = new Random();
            var index = rand.Next(0, 3);

            _outputHelper.WriteLine(_quizPage.GetAnswerText(index));
            _quizPage.ClickAnswer(index);

            Thread.Sleep(msTimeout);

            if (_quizPage.QuestionDisplayed && _quizPage.QuestionText.Text != question)
            {
                Assert.Equal(cat, _quizPage.QuestionCategoryBadge.Text);
            }
            else
            {
                _outputHelper.WriteLine(_quizPage.ScoreText.Text);
                Assert.Equal("Game over!", _quizPage.GameOverText.Text);
            }
        }

        [Fact]
        public void TestJoker()
        {
            _loginPage.Navigate();

            _loginPage.PopulateUsername("TestUser");
            _loginPage.PopulatePassword("user123");
            _loginPage.ClickLogin();

            Thread.Sleep(msTimeout);

            var cat = "Automotives";

            _quizPage.SelectCategory(cat);
            _quizPage.ClickPlayButton();

            Thread.Sleep(msTimeout);
            
            _quizPage.UseJoker();

            Thread.Sleep(msTimeout);

            var availableAnswers = _quizPage.AnswerButtons.Count(b => b.Enabled);
            Assert.Equal(2, availableAnswers);
        }

        public void Dispose()
        {
            _driver.Quit();
            _driver.Dispose();
        }
    }
}