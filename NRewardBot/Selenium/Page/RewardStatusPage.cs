using System;
using NRewardBot.Selenium.Elements;
using OpenQA.Selenium;

namespace NRewardBot.Selenium.Page
{
    internal class RewardStatusPage : PageBase
    {
        #region Logger

        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();

        #endregion

        private static readonly Random Random = new Random(DateTime.Now.Millisecond);
        private const string PageUrl = "http://www.bing.com/rewardsapp/bepflyoutpage?style=chromeextension";
        private const string CreditsElementId = "credits";

        protected IWebElement CreditsElement => this.Driver.FindElement(By.Id(CreditsElementId));

        public RewardStatusPage(IWebDriver driver) : base(driver)
        {
        }

        public static RewardStatusPage NavigateTo(IWebDriver driver)
        {
            driver.Navigate().GoToUrl(PageUrl);

            var rewardStatusPage = new RewardStatusPage(driver);
            driver.WaitUntilElementIsDisplayed(By.ClassName("pcsearch"), throwOnTimeout: false, TimeSpan.FromSeconds(4));
            return rewardStatusPage;
        }

        public bool IsLevelOne()
        {
            var credits = CreditsElement;
            var searchElement = credits.WaitUntilElementIsDisplayed(By.ClassName("allsearch"), throwOnTimeout:false, TimeSpan.FromMilliseconds(500));

            return searchElement != null;
        }

        public bool AllSearchComplete()
        {
            return SearchComplete(By.ClassName("allsearch"));
        }

        public bool PcSearchComplete()
        {
            return SearchComplete(By.ClassName("pcsearch"));
        }

        public bool MobileSearchComplete()
        {
            return SearchComplete(By.ClassName("mobilesearch"));
        }

        private bool SearchComplete(By rootElement)
        {
            var tally = GetSearchTally(rootElement);
            var total = GetSearchTotal(rootElement);

            Log.Info("Completed {tally} of {total}", tally, total);
            return tally >= total;
        }

        public int GetPcSearchTally()
        {
            return this.GetSearchTally(By.ClassName("pcsearch"));
        }

        private int GetSearchTally(By rootElement)
        {
            var credits = CreditsElement;
            var searchElement = credits.WaitUntilElementIsDisplayed(rootElement);
            var earnedElement = searchElement.WaitUntilElementIsDisplayed(By.ClassName("earned"));

            var pointsValue = earnedElement.Text;

            if (!int.TryParse(pointsValue, out var points))
            {
                Log.Info("Could not determine points tally for PC Search: \"{pointsValue}\"", pointsValue);
                return 0;
            }

            return points;
        }

        public int GetMobileSearchTally()
        {
            return this.GetSearchTally(By.ClassName("mobilesearch"));
        }

        public int GetPcSearchTotal()
        {
            return GetSearchTotal(By.ClassName("pcsearch"));
        }

        public int GetMobileTotal()
        {
            return GetSearchTotal(By.ClassName("mobilesearch"));
        }

        private int GetSearchTotal(By rootElement)
        {
            var credits = CreditsElement;
            var searchElement = credits.WaitUntilElementIsDisplayed(rootElement);
            var searchText = searchElement.Text;
            var slashIdx = searchText.LastIndexOf('/');

            var pointsValue = searchText.Substring(slashIdx + 1);

            if (!int.TryParse(pointsValue, out var points))
            {
                Log.Info("Could not determine points total for PC Search: \"{pointsValue}\"", pointsValue);
                return 0;
            }

            return points;
        }


    }
}