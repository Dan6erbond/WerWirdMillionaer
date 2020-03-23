using OpenQA.Selenium;

namespace WhoWantsToBeAMillionaire.Tests.SeleniumTests
{
    public class LoginPage : PageObjectModel
    {
        private const string Uri = "";
        
        private IWebElement UsernameInput => Driver.FindElement(By.Id("formBasicUsername"));
        private IWebElement PasswordInput => Driver.FindElement(By.Id("formBasicPassword"));
        private IWebElement LoginButton =>
            Driver.FindElement(By.CssSelector("#root > div > div > form > div:nth-child(3) > button"));

        public LoginPage(IWebDriver driver) : base(driver, Uri)
        {
        }
        
        public void PopulateUsername(string username) => UsernameInput.SendKeys(username);
        public void PopulatePassword(string password) => PasswordInput.SendKeys(password);
        public void ClickLogin() => LoginButton.Click();
    }
}