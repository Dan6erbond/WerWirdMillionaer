using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WhoWantsToBeAMillionaire.Models.Data.Quiz;
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
        private readonly AdminPage _adminPage;
        private readonly LeaderboardPage _leaderboardPage;

        public AutomatedUiTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
            _driver = new ChromeDriver();

            _loginPage = new LoginPage(_driver);
            _quizPage = new QuizPage(_driver);
            _adminPage = new AdminPage(_driver);
            _leaderboardPage = new LeaderboardPage(_driver);
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

            var question = _quizPage.QuestionText.Text.Substring(0,
                    _quizPage.QuestionText.Text.LastIndexOf(_quizPage.QuestionCategoryBadge.Text,
                        StringComparison.Ordinal))
                .Trim();
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

        [Fact]
        public void TestAdminLogin()
        {
            _loginPage.Navigate();

            _loginPage.PopulateUsername("TestAdmin");
            _loginPage.PopulatePassword("admin123");
            _loginPage.ClickLogin();

            Thread.Sleep(msTimeout);

            Assert.Contains("admin", _loginPage.Url);
        }

        [Fact]
        public void TestAddQuestion()
        {
            _loginPage.Navigate();

            _loginPage.PopulateUsername("TestAdmin");
            _loginPage.PopulatePassword("admin123");
            _loginPage.ClickLogin();

            Thread.Sleep(msTimeout);

            var numQuestions = _adminPage.QuestionTitles.Count;

            _adminPage.ClickAddQuestion();

            _adminPage.SetQuestion("Testfrage");

            _adminPage.SetAnswer(0, "Antwort 1");
            _adminPage.SetAnswer(1, "Antwort 2");
            _adminPage.SetAnswer(2, "Antwort 3");
            _adminPage.SetAnswer(3, "Antwort 4");

            _adminPage.SubmitQuestion();

            Assert.Equal(numQuestions + 1, _adminPage.QuestionTitles.Count);

            Thread.Sleep(msTimeout);

            _adminPage.DeleteQuestion(_adminPage.QuestionTitles.Count - 1);

            Thread.Sleep(msTimeout);

            Assert.Equal(numQuestions, _adminPage.QuestionTitles.Count);
        }

        [Fact]
        public void TestEditQuestion()
        {
            _loginPage.Navigate();

            _loginPage.PopulateUsername("TestAdmin");
            _loginPage.PopulatePassword("admin123");
            _loginPage.ClickLogin();

            Thread.Sleep(msTimeout);

            var orgQuestion = _adminPage.QuestionTitles.First().Text;
            var newQuestion = "Geänderte Frage";

            _adminPage.SetQuestion(newQuestion);
            _adminPage.SubmitQuestion();

            Assert.Equal(newQuestion, _adminPage.QuestionTitles.First().Text);

            Thread.Sleep(msTimeout);

            _adminPage.SetQuestion(orgQuestion);
            _adminPage.SubmitQuestion();

            Thread.Sleep(msTimeout);

            Assert.Equal(orgQuestion, _adminPage.QuestionTitles.First().Text);
        }

        [Fact]
        public void TestAdminLeaderboard()
        {
            _loginPage.Navigate();

            _loginPage.PopulateUsername("TestAdmin");
            _loginPage.PopulatePassword("admin123");
            _loginPage.ClickLogin();

            Thread.Sleep(msTimeout);

            _leaderboardPage.Navigate();
            
            Assert.True(_leaderboardPage.DeleteButtons.Count > 0);
        }

        [Fact]
        public void TestDeleteGame()
        {
            _loginPage.Navigate();

            _loginPage.PopulateUsername("TestAdmin");
            _loginPage.PopulatePassword("admin123");
            _loginPage.ClickLogin();

            Thread.Sleep(msTimeout);

            _leaderboardPage.Navigate();

            Thread.Sleep(msTimeout);

            Assert.True(_leaderboardPage.DeleteButtons.Count > 0);

            if (_leaderboardPage.DeleteButtons.Count <= 0) return;
            
            var orgGames = _leaderboardPage.DeleteButtons.Count;
            _leaderboardPage.DeleteGame(0);
            Assert.Equal(orgGames - 1, _leaderboardPage.DeleteButtons.Count);
        }

        public void Dispose()
        {
            _driver.Quit();
            _driver.Dispose();
        }
    }
}