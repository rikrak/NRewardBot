using System;
using NRewardBot.Selenium.Elements;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace NRewardBot.Selenium.Page
{
    public class BingSearchPageBase : PageBase
    {
        #region Logger
        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();
        #endregion

        #region Constructor
        protected BingSearchPageBase(IWebDriver driver) : base(driver)
        {
        }
        #endregion

        public TPage EnsureLoggedIn<TPage>()
            where TPage: BingSearchPageBase
        {
            Log.Trace("EnsureLoggedIn");
            // check for Desktop and mobile version of login button
            var loginButton = this.Driver.WaitUntilElementIsDisplayed(By.Id("id_l"), throwOnTimeout: false, timeout: TimeSpan.FromSeconds(1));
            if (loginButton == null)
            {
                Log.Debug("Checking for mobile login");
                var burgerMenu = this.Driver.WaitUntilElementIsDisplayed(By.Id("mHamburger"), throwOnTimeout: false, timeout: TimeSpan.FromSeconds(1));
                burgerMenu?.Click();
                loginButton = this.Driver.WaitUntilElementIsDisplayed(By.Id("hb_s"), throwOnTimeout: false, timeout: TimeSpan.FromSeconds(1));
            }

            if (loginButton == null)
            {
                Log.Debug("No login button found");
            }
            else
            {
                // just wait a tick - sometimes the login button click doesn't work if done too quickly
                this.Driver.DoWait(1);

                Log.Debug("Clicking login button");
                loginButton.Click();
            }
            return (TPage)this;
        }

        public TPage AcceptCookies<TPage>() 
            where TPage : BingSearchPageBase
        {
            Log.Trace("AcceptCookies");

            // I think the "accept Cookies" button moves around a little bit, so let the page settle first
            this.Driver.DoWait(2);

            var cookieButton = this.Driver.WaitUntilElementIsDisplayed(By.Id("bnp_btn_accept"), throwOnTimeout: false, timeout: TimeSpan.FromSeconds(1));
            if (cookieButton != null)
            {
                var i = 3;
                while (i-- > 0)
                {
                    try
                    {
                        if (cookieButton != null)
                        {
                            cookieButton.Click();
                            i = 0;
                            Log.Trace("Cookies Accepted");
                        }
                    }
                    catch (OpenQA.Selenium.NoSuchElementException e)
                    {
                        Log.Debug("Cookie Button was stale");
                        cookieButton = this.Driver.WaitUntilElementIsDisplayed(By.Id("bnp_btn_accept"), throwOnTimeout: false, timeout: TimeSpan.FromSeconds(2));
                    }
                }
            }
            return (TPage)this;
        }
    }
}