using System;
using OpenQA.Selenium;

namespace NRewardBot.Selenium.Page
{
    public class BingSearchPage : BingSearchPageBase
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

        public BingSearchPage EnsureLoggedIn() => this.EnsureLoggedIn<BingSearchPage>();
        public BingSearchPage AcceptCookies() => this.AcceptCookies<BingSearchPage>();

        public BingSearchPage Search(string term)
        {
            Log.Info("Searching for: {term}", term);
            this.Driver.Navigate().GoToUrl(PageUrl);
            this.Driver.WaitUntilElementIsDisplayed(By.Id(SearchTermElementId));
            this.Driver.DoWait(1);
            this.SearchTermInputElement.ClearAndSendKeys(term);
            this.Driver.DoWait(1);
            this.SearchTermInputElement.SendKeys(Keys.Return);
            var wait = Random.Next(240, 360);
            this.Driver.DoWait(wait); // human-like delay

            return this;
        }
    }
}