using System;
using System.Linq;
using NRewardBot.Selenium.Elements;
using OpenQA.Selenium;

namespace NRewardBot.Selenium.Page
{
    public class OfferLinkElement : ElementBase
    {
        private readonly IWebElement _element;

        public OfferLinkElement(IWebDriver driver, IWebElement element) : base(driver)
        {
            _element = element ?? throw new ArgumentNullException(nameof(element));
        }

        public IOfferPage Click()
        {
            var mainWindowHandle = this.Driver.WindowHandles.Last();
            this._element.Click();
            // a new window should open
            this.Driver.DoWait(2);

            var newWindowHandle = this.Driver.WindowHandles.Last();

            if (newWindowHandle != mainWindowHandle)
            {
                var newWindow = this.Driver.SwitchTo().Window(newWindowHandle);
                CheckForSigninPrompt();

                if (DailyPollOfferPage.IsDailyPollPage(newWindow))
                {
                    return new DailyPollOfferPage(newWindow);
                }

                if (LighteningQuizPage.IsLightningQuizPage(newWindow))
                {
                    return new LighteningQuizPage(newWindow);
                }

                return new ExploreDailyPage(newWindow);
            }

            return null;
        }

        private void CheckForSigninPrompt()
        {
            // the signin page that is displayed can depend on the browser type that is used
            // this is currently configured for Desktop.
            // Although it doesn't work for some reason (3rd party cookies?)
            try
            {
                var signinElement = this.Driver.WaitUntilElementIsDisplayed(By.ClassName("simpleSignIn"), throwOnTimeout: false, TimeSpan.FromSeconds(2));
                if (signinElement != null)
                {
                    var signInButton = this.Driver.FindElement(By.LinkText("Sign in"));
                    signInButton.Click();
                }
            }
            catch (NoSuchElementException e)
            {
                
            }
        }
    }
}