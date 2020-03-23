using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace WhoWantsToBeAMillionaire.Tests
{
    public static class WebDriverExtensions
    {
        public static void WaitForComponents(this IWebDriver driver)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
        }

        public static bool ElementExists(this IWebDriver driver, By by)
        {
            return driver.FindElements(by).Count != 0;
        }
    }
}