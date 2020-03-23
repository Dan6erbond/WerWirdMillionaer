using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace WhoWantsToBeAMillionaire.AutomatedUiTests
{
    public static class WebDriverExtensions
    {
        public static void WaitForComponents(this IWebDriver driver)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
        }
    }
}