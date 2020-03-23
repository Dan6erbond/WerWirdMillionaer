using System;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Xunit;
using Xunit.Abstractions;

namespace WhoWantsToBeAMillionaire.AutomatedUiTests
{
    public class AutomatedUiTests : IDisposable
    {
        private readonly IWebDriver _driver;
        private readonly ITestOutputHelper _outputHelper;

        private readonly LoginPage _loginPage;
        
        public AutomatedUiTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
            _driver = new ChromeDriver();
            
            _loginPage = new LoginPage(_driver);
        }

        [Fact]
        public void TestSiteLoad()
        {
            _loginPage.Navigate();

            Assert.Equal("WhoWantsToBeAMillionaire", _driver.Title);
            Assert.Contains("Log in", _driver.PageSource);
        }

        [Fact]
        public void TestLogin()
        {
            _loginPage.Navigate();
            
            _loginPage.PopulateUsername("TestUser");
            _loginPage.PopulateUsername("user123");
            _loginPage.ClickLogin();

            Thread.Sleep(50);
            
            var url = _driver.Url;
            Assert.Contains("quiz", url);
        }
        
        public void Dispose()
        {
            _driver.Quit();
            _driver.Dispose();
        }
    }
}