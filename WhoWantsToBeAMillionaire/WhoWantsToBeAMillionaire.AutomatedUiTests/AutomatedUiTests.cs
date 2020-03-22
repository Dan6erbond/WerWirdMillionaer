using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Xunit;

namespace WhoWantsToBeAMillionaire.AutomatedUiTests
{
    public class AutomatedUiTests : IDisposable
    {
        private readonly IWebDriver _driver;
        public AutomatedUiTests()
        {
            _driver = new ChromeDriver();
        }
        
        public void Dispose()
        {
            _driver.Quit();
            _driver.Dispose();
        }

        [Fact]
        public void TestSiteLoad()
        {
            _driver.Navigate().GoToUrl("https://localhost:44309");
            
            Assert.Equal("WhoWantsToBeAMillionaire", _driver.Title);
            Assert.Contains("Log in", _driver.PageSource);
        }
    }
}