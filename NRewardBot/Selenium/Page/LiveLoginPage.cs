using System;
using NRewardBot.Config;
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
        private const string AuthAppId = "idRemoteNGC_DisplaySign";
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
            var multiFactor = this.Driver.WaitUntilElementIsDisplayed(By.Id(DoNotShowThisAgainId), throwOnTimeout: false, TimeSpan.FromSeconds(20));
            while (multiFactor != null)
            {
                // wait until the multi factor has been acknowledged by the User
                Log.Warn("Waiting for Multi-Factor authentication");
                multiFactor = this.Driver.WaitUntilElementIsDisplayed(By.Id(DoNotShowThisAgainId), throwOnTimeout: false, TimeSpan.FromSeconds(5));
            }
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

        public void CheckForPasswordError()
        {
            var element = this.Driver.WaitUntilElementIsDisplayed(By.Id("passwordError"), throwOnTimeout: false, TimeSpan.FromSeconds(1));
            if (element != null)
            {
                throw new Exception("The password did not work :-(");
            }
        }

        public LiveLoginPage ProcessLogin(ICredentials credentials)
        {
            this.WithUsername(credentials.Username)
                .PressNext();

            var authStrategy = GetAuthMechanism();

            if (authStrategy == AuthenticationMechanism.Password)
            {

                this.WithPassword(credentials.Password)
                    .PressSubmit()
                    //.DoMultiFactorAuth()
                    .PressStayLoggedIn();
                this.CheckForPasswordError();
            }

            if (authStrategy == AuthenticationMechanism.AuthenticatorApp)
            {
                // do what?
                var timeout = TimeSpan.FromMinutes(20);
                // wait until the auth app page is no longer displayed
                this.Driver.WaitUntil(d =>
                {
                    try
                    {
                        var _ = d.FindElement(By.Id(AuthAppId));
                        Log.Debug("Auth App page is still displayed");
                    }
                    catch (StaleElementReferenceException)
                    {
                        Log.Debug("Auth App page is no longer displayed");
                        return true;
                    }
                    catch (NoSuchElementException)
                    {
                        Log.Debug("Auth App page is no longer displayed");
                        return true;
                    }

                    return false;
                }, throwOnTimeout: false, timeout: timeout);
            }

            if (authStrategy == AuthenticationMechanism.Undefined)
            {
                throw new Exception("Could not determine the authentication mechanism");
            }

            return this;
        }

        private AuthenticationMechanism GetAuthMechanism()
        {
            var timeout = TimeSpan.FromSeconds(5);
            Log.Debug($"Determining the Auth Mechanism");
            AuthenticationMechanism result = AuthenticationMechanism.Undefined;

            this.Driver.WaitUntil(d =>
            {
                var pwdLocator = By.Name(PasswordInputName);
                var appAuthLocator = By.Id(AuthAppId);

                IWebElement element = null;
                
                int i = 0;

                while (element == null && i++ < 100)
                {
                    this.Driver.DoWait();
                    var elementLocator = i % 2 == 0 ? pwdLocator : appAuthLocator;
                    try
                    {
                        element = this.Driver.FindElement(elementLocator);
                        var displayed = element?.Displayed;
                        Log.Debug(() => $"{elementLocator} displayed: {displayed}");
                    }
                    catch (StaleElementReferenceException)
                    {
                        Log.Debug(() => $"Element {elementLocator} is stale");
                    }
                    catch (NoSuchElementException)
                    {
                        Log.Debug(() => $"Element {elementLocator} cannot be found");
                    }

                }

                if (element != null)
                {
                    var name = element.GetAttribute("name");
                    var id = element.GetAttribute("id");

                    if (string.Equals(name, PasswordInputName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        result = AuthenticationMechanism.Password;
                    }
                    if (string.Equals(id, AuthAppId, StringComparison.InvariantCultureIgnoreCase))
                    {
                        result = AuthenticationMechanism.AuthenticatorApp;
                    }
                }
                Log.Info(() => $"Auth mechanism is {result}");
                return element != null;
            }, throwOnTimeout: false, timeout);


            return result;
        }

        private enum AuthenticationMechanism
        {
            Undefined,
            Password,
            AuthenticatorApp
        }
    }
}
