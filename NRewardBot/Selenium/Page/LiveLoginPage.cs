using NRewardBot.Selenium.Elements;
using OpenQA.Selenium;

namespace NRewardBot.Selenium.Page
{
    internal class LiveLoginPage : PageBase
    {
        private const string PageUrl = "https://login.live.com/";

        private const string UsernameInputName = "loginfmt";
        private const string PasswordInputName = "passwd";
        private const string SubmitButtonId = "idSIButton9";
        #region Constructor
        public LiveLoginPage(IWebDriver driver) : base(driver)
        {
        }
        #endregion


        public static LiveLoginPage NavigateTo(IWebDriver driver)
        {
            driver.Navigate().GoToUrl(PageUrl);
            
            return new LiveLoginPage(driver);
        }

        protected IWebElement UsernameInputElement => this.Driver.FindElement(By.Name(UsernameInputName));
        protected IWebElement PasswordInputElement => this.Driver.FindElement(By.Name(PasswordInputName));
        protected IWebElement SubmitButtonElement => this.Driver.FindElement(By.Id(SubmitButtonId));

        public LiveLoginPage WithUsername(string value)
        {
            this.Driver.WaitUntilElementIsDisplayed(By.Name(UsernameInputName));
            
            UsernameInputElement.ClearAndSendKeys(value);
            return this;
        }

        public LiveLoginPage WithPassword(string value)
        {
            this.Driver.WaitUntilElementIsDisplayed(By.Name(PasswordInputName));
            PasswordInputElement.ClearAndSendKeys(value);
            return this;
        }

        public LiveLoginPage PressNext()
        {
            this.SubmitButtonElement.Click();
            return this;
        }

        public LiveLoginPage PressSubmit()
        {
            this.SubmitButtonElement.Click();
            return this;
        }

    }
}
