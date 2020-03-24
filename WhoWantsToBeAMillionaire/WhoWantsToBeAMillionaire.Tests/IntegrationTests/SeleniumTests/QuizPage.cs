using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;

namespace WhoWantsToBeAMillionaire.Tests.SeleniumTests
{
    public class QuizPage : PageObjectModel
    {
        private const string Uri = "quiz";

        private IReadOnlyList<IWebElement> CategoryButtons =>
            Driver.FindElements(By.CssSelector("#root > div > div > div > div > div > label"));

        private IWebElement PlayButton => Driver.FindElement(By.XPath("//*[@id=\"root\"]/div/div/div/button"));

        public bool QuestionDisplayed =>
            Driver.ElementExists(By.XPath("//*[@id=\"root\"]/div/div/div/div[2]/div[1]/h1"));

        public IWebElement QuestionText =>
            Driver.FindElement(By.XPath("//*[@id=\"root\"]/div/div/div/div[2]/div[1]/h1"));

        public IWebElement QuestionCategoryBadge =>
            Driver.FindElement(By.XPath("//*[@id=\"root\"]/div/div/div/div[2]/div[1]/h1/span"));

        private IWebElement UseJokerButton =>
            Driver.FindElement(By.XPath("//*[@id=\"root\"]/div/div/div/div[1]/button"));

        private IWebElement AnswerButtonsContainer =>
            Driver.FindElement(By.XPath("//*[@id=\"root\"]/div/div/div/div[2]/div[2]"));
        
        public List<IWebElement> AnswerButtons => new List<IWebElement>
        {
            AnswerButtonsContainer.FindElement(By.CssSelector("div:nth-child(1) > div:nth-child(1) > button")),
            AnswerButtonsContainer.FindElement(By.CssSelector("div:nth-child(1) > div:nth-child(2) > button")),
            AnswerButtonsContainer.FindElement(By.CssSelector("div:nth-child(3) > div:nth-child(1) > button")),
            AnswerButtonsContainer.FindElement(By.CssSelector("div:nth-child(3) > div:nth-child(2) > button"))
        };

        private IWebElement EndGameButton =>
            Driver.FindElement(By.XPath("//*[@id=\"root\"]/div/div/div/div[3]/button"));

        private IWebElement TimePointsText => Driver.FindElement(By.XPath("//*[@id=\"root\"]/div/div/div/p"));

        private IWebElement GameOverJumbotron => Driver.FindElement(By.XPath("//*[@id=\"root\"]/div/div/div/div"));

        public IWebElement GameOverText =>
            GameOverJumbotron.FindElement(By.CssSelector("h1"));

        public IWebElement ScoreText =>
            GameOverJumbotron.FindElement(By.CssSelector("p:nth-child(4)"));

        public QuizPage(IWebDriver driver) : base(driver, Uri)
        {
        }

        public void SelectCategory(string name) => CategoryButtons.First(b => b.Text == name).Click();
        public void ClickPlayButton() => PlayButton.Click();

        public void ClickAnswer(int index) => AnswerButtons[index].Click();
        public string GetAnswerText(int index) => AnswerButtons[index].Text;

        public void UseJoker() => UseJokerButton.Click();
    }
}