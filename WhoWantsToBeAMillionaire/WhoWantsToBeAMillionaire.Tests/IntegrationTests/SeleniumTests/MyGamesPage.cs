using System.Collections.Generic;
using OpenQA.Selenium;

namespace WhoWantsToBeAMillionaire.Tests.SeleniumTests
{
    public class MyGamesPage : PageObjectModel
    {
        private const string Uri = "games";

        public IReadOnlyList<IWebElement> GameTitles =>
            Driver.FindElements(By.CssSelector("#root > div > div > div > div.card > div.card-header > button"));

        public MyGamesPage(IWebDriver driver) : base(driver, Uri)
        {
        }

        public new void Navigate() => Driver.FindElement(By.LinkText("My Games")).Click();
    }
}