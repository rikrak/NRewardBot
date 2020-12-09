using System;
using OpenQA.Selenium;

namespace NRewardBot.Selenium.Page
{
    internal class RewardStatusPageV1 : RewardStatusPage
    {
        #region Logger

        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();

        #endregion

        #region COnstructor

        public RewardStatusPageV1(IWebDriver driver) : base(driver)
        {
        }

        #endregion

        public const string CreditsElementId = "credits";

        protected IWebElement CreditsElement => this.Driver.WaitUntilElementIsAvailable(By.Id(CreditsElementId), throwOnTimeout: false);


        public override bool IsLevelOne()
        {
            var credits = CreditsElement;
            var searchElement = credits?.WaitUntilElementIsDisplayed(By.ClassName("allsearch"), throwOnTimeout: false, TimeSpan.FromMilliseconds(500));

            return searchElement != null;
        }

        /// <summary>
        /// used for level one accounts
        /// </summary>
        /// <returns></returns>
        public override bool AllSearchComplete()
        {
            return SearchComplete(By.ClassName("allsearch"));
        }

        public override bool PcSearchComplete()
        {
            return SearchComplete(By.ClassName("pcsearch"));
        }

        public override bool MobileSearchComplete()
        {
            return SearchComplete(By.ClassName("mobilesearch"));
        }

        private bool SearchComplete(By rootElement)
        {
            var tally = GetSearchTally(rootElement);
            var total = GetSearchTotal(rootElement);

            Log.Info("Completed {tally} of {total}", tally, total);
            return total != 0 && tally >= total;
        }

        private int GetSearchTally(By rootElement)
        {
            var credits = CreditsElement;
            if (credits == null) return 0;
            var searchElement = credits.WaitUntilElementIsAvailable(rootElement, throwOnTimeout: false);
            var earnedElement = searchElement?.WaitUntilElementIsAvailable(By.ClassName("earned"), throwOnTimeout: false);

            var pointsValue = earnedElement?.Text ?? "0";

            if (!int.TryParse(pointsValue, out var points))
            {
                Log.Info("Could not determine points tally for PC Search: \"{pointsValue}\"", pointsValue);
                return 0;
            }

            return points;
        }

        private int GetSearchTotal(By rootElement)
        {
            var credits = CreditsElement;
            if (credits == null)
            {
                return 0;
            }
            var searchElement = credits.WaitUntilElementIsAvailable(rootElement, throwOnTimeout: false);
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