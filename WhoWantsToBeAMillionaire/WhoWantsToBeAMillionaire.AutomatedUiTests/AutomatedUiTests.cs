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
        
        public AutomatedUiTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
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

        [Fact]
        public void TestLogin()
        {
            _driver.Navigate().GoToUrl("https://localhost:44309");
            
            _driver.FindElement(By.Id("formBasicUsername")).SendKeys("TestUser");
            _driver.FindElement(By.Id("formBasicPassword")).SendKeys("user123");
            _driver.FindElement(By.CssSelector("#root > div > div > form > div:nth-child(3) > button")).Click();

            Thread.Sleep(50);
            
            var url = _driver.Url;
            Assert.Contains("quiz", url);
        }
    }
}