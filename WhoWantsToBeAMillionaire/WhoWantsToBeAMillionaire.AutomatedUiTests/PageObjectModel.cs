using OpenQA.Selenium;

namespace WhoWantsToBeAMillionaire.AutomatedUiTests
{
    public abstract class PageObjectModel
    {
        protected readonly IWebDriver Driver;

        private const string uriBase = "https://localhost:5001/";
        private readonly string _uri;
        private string Uri => uriBase + _uri;

        public string Title => Driver.Title;
        public string Source => Driver.PageSource;

        protected PageObjectModel(IWebDriver driver, string uri)
        {
            Driver = driver;
            _uri = uri;
        }

        public void Navigate() => Driver.Navigate().GoToUrl(Uri);
    }
}