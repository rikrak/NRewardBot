using System;
using NRewardBot.Selenium.Elements;
using OpenQA.Selenium;

namespace NRewardBot.Selenium.Page
{
    internal class LiveLoginPage : PageBase
    {
        #region Logger
        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();
        #endregion

        private const string PageUrl = "https://login.live.com/";

        private const string UsernameInputName = "loginfmt";
        private const string PasswordInputName = "passwd";
        private const string SubmitButtonId = "idSIButton9";
        private const string DoNotShowThisAgainId = "KmsiCheckboxField";

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
            Log.Info("Entering username");
            this.Driver.WaitUntilElementIsDisplayed(By.Name(UsernameInputName));
            
            UsernameInputElement.ClearAndSendKeys(value);
            return this;
        }

        public LiveLoginPage WithPassword(string value)
        {
            Log.Info("Entering password");
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

        public LiveLoginPage DoMultiFactorAuth()
        {
            Log.Warn("Waiting for Multi-Factor authentication");
            // after the multi-factor auth, the "Stay Logged in" page is displayed.
            // when this happens, we can move on
            // If multi-factor auth is not enabled, then the "Stay Logged in" page is displayed and we move on too:-)
            this.Driver.WaitUntilElementIsDisplayed(By.Id(DoNotShowThisAgainId), throwOnTimeout: false, TimeSpan.FromSeconds(20));
            Log.Info("Multi-Factor authentication - Done!");
            return this;
        }

        public LiveLoginPage PressStayLoggedIn()
        {
            var element = this.Driver.WaitUntilElementIsDisplayed(By.Id(DoNotShowThisAgainId), throwOnTimeout: false, TimeSpan.FromSeconds(3));

            if (element != null)
            {
                this.SubmitButtonElement.Click();
            }

            return this;
        }

    }
}
