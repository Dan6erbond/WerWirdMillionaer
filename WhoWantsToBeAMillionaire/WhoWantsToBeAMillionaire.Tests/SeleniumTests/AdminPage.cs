using System.Collections.Generic;
using OpenQA.Selenium;

namespace WhoWantsToBeAMillionaire.Tests.SeleniumTests
{
    public class AdminPage : PageObjectModel
    {
        private const string Uri = "admin";

        private IWebElement AddQuestionButton =>
            Driver.FindElement(By.XPath("//*[@id=\"root\"]/div/div/div/div[2]/div/div[2]/button"));

        public IReadOnlyList<IWebElement> QuestionTitles => Driver.FindElements(By.CssSelector(
            "#root > div > div > div > div:nth-child(3) > div > div.accordion > div.card > div.card-header > button:nth-child(1)"));

        private IReadOnlyList<IWebElement> DeleteQuestionButtons => Driver.FindElements(By.CssSelector(
            "#root > div > div > div > div:nth-child(3) > div > div.accordion > div.card > div.card-header > button:nth-child(2)"));

        private IWebElement OpenQuestion => Driver.FindElement(By.CssSelector(
            "#root > div > div > div > div:nth-child(3) > div > div.accordion > div.card > div.collapse.show"));

        private IWebElement QuestionInput => OpenQuestion.FindElement(By.CssSelector("#question"));

        private List<IWebElement> AnswerInputs => new List<IWebElement>
        {
            OpenQuestion.FindElement(By.CssSelector(
                "div > form > div:nth-child(2) > table > tbody > tr:nth-child(1) > td:nth-child(2) > textarea:nth-child(1)")),
            OpenQuestion.FindElement(By.CssSelector(
                "div > form > div:nth-child(2) > table > tbody > tr:nth-child(2) > td:nth-child(2) > textarea:nth-child(1)")),
            OpenQuestion.FindElement(By.CssSelector(
                "div > form > div:nth-child(2) > table > tbody > tr:nth-child(3) > td:nth-child(2) > textarea:nth-child(1)")),
            OpenQuestion.FindElement(By.CssSelector(
                "div > form > div:nth-child(2) > table > tbody > tr:nth-child(4) > td:nth-child(2) > textarea:nth-child(1)"))
        };

        private List<IWebElement> AnswerRadios => new List<IWebElement>
        {
            OpenQuestion.FindElement(By.CssSelector(
                "div > form > div:nth-child(2) > table > tbody > tr:nth-child(1) > td:nth-child(3) > div > input:nth-child(1)")),
            OpenQuestion.FindElement(By.CssSelector(
                "div > form > div:nth-child(2) > table > tbody > tr:nth-child(2) > td:nth-child(3) > div > input:nth-child(1)")),
            OpenQuestion.FindElement(By.CssSelector(
                "div > form > div:nth-child(2) > table > tbody > tr:nth-child(3) > td:nth-child(3) > div > input:nth-child(1)")),
            OpenQuestion.FindElement(By.CssSelector(
                "div > form > div:nth-child(2) > table > tbody > tr:nth-child(4) > td:nth-child(3) > div > input:nth-child(1)"))
        };

        public IWebElement QuestionSubmitButton => OpenQuestion.FindElement(By.CssSelector(
            "div > form > button"));

        public AdminPage(IWebDriver driver) : base(driver, Uri)
        {
        }

        public void ClickAddQuestion() => AddQuestionButton.Click();
        public void SetQuestion(string question) => QuestionInput.SendKeys(question);
        public void SetAnswer(int index, string answer) => AnswerInputs[index].SendKeys(answer);

        public void SetCorrectAnswer(int index) =>
            ((IJavaScriptExecutor) Driver).ExecuteScript("arguments[0].checked = true;", AnswerRadios[index]);

        public void SubmitQuestion() => QuestionSubmitButton.Click();
        public void DeleteQuestion(int index) => DeleteQuestionButtons[index].Click();
    }
}