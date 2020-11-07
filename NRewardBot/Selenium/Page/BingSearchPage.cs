using System;
using NRewardBot.Selenium.Elements;
using OpenQA.Selenium;

namespace NRewardBot.Selenium.Page
{
    internal class BingSearchPage : PageBase
    {
        #region Logger
        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();
        #endregion

        private static readonly Random Random = new Random(DateTime.Now.Millisecond);
        private const string PageUrl = "https://www.bing.com/search";
        private const string SearchTermElementId = "sb_form_q";

        protected IWebElement SearchTermInputElement => this.Driver.FindElement(By.Id(SearchTermElementId));

        public BingSearchPage(IWebDriver driver) : base(driver)
        {
        }

        public static BingSearchPage NavigateTo(IWebDriver driver)
        {
            driver.Navigate().GoToUrl(PageUrl);

            return new BingSearchPage(driver);
        }

        public BingSearchPage EnsureLoggedIn()
        {
            Log.Trace("EnsureLoggedIn");
            // check for Desktop and mobile version of login button
            var loginButton = this.Driver.WaitUntilElementIsDisplayed(By.Id("id_l"), throwOnTimeout: false, timeout: TimeSpan.FromSeconds(1));
            if (loginButton == null)
            {
                var burgerMenu = this.Driver.WaitUntilElementIsDisplayed(By.Id("mHamburger"), throwOnTimeout: false, timeout: TimeSpan.FromSeconds(1));
                burgerMenu?.Click();
                loginButton = this.Driver.WaitUntilElementIsDisplayed(By.Id("hb_s"), throwOnTimeout: false, timeout: TimeSpan.FromSeconds(1));
            }
                
            loginButton?.Click();
            return this;
        }

        public BingSearchPage AcceptCookies()
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
            return this;
        }

        public BingSearchPage Search(string term)
        {
            Log.Info("Searching for: {term}", term);
            this.Driver.Navigate().GoToUrl(PageUrl);
            this.Driver.WaitUntilElementIsDisplayed(By.Id(SearchTermElementId));
            this.SearchTermInputElement.ClearAndSendKeys(term);
            this.Driver.DoWait(1);
            this.SearchTermInputElement.SendKeys(Keys.Return);
            var wait = Random.Next(3, 8);
            this.Driver.DoWait(wait); // human-like delay

            return this;
        }
    }
}