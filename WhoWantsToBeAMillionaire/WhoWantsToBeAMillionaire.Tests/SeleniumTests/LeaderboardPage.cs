using System.Collections.Generic;
using OpenQA.Selenium;

namespace WhoWantsToBeAMillionaire.Tests.SeleniumTests
{
    public class LeaderboardPage : PageObjectModel
    {
        private const string Uri = "leaderboard";

        public IReadOnlyList<IWebElement> DeleteButtons =>
            Driver.FindElements(
                By.CssSelector("#root > div > div > div > table > tbody > tr > td:nth-child(8) > button"));

        public LeaderboardPage(IWebDriver driver) : base(driver, Uri)
        {
        }

        public new void Navigate() => Driver.FindElement(By.LinkText("Leaderboard")).Click();

        public void DeleteGame(int index) => DeleteButtons[index].Click();
    }
}