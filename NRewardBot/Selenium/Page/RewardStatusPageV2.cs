using System;
using OpenQA.Selenium;

namespace NRewardBot.Selenium.Page
{
    internal class RewardStatusPageV2 : RewardStatusPage
    {
        #region Logger

        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();

        #endregion

        #region COnstructor

        public RewardStatusPageV2(IWebDriver driver) : base(driver)
        {
        }

        #endregion

        public const string PageElementId = "modern-flyout";

        public const string CreditsElementId = "credits";

        protected IWebElement CreditsElement => this.Driver.WaitUntilElementIsAvailable(By.Id(CreditsElementId), throwOnTimeout: false);


        public override bool IsLevelOne()
        {
            var credits = CreditsElement;
            var searchElement = credits?.WaitUntilElementIsDisplayed(By.ClassName("allsearch"), throwOnTimeout: false, TimeSpan.FromMilliseconds(500));

            return searchElement != null;
        }

        /// <summary>
        /// Used for level one accounts
        /// </summary>
        /// <returns></returns>
        public override bool AllSearchComplete()
        {
            // not sure what the structure of the page is, so return false
            return false;
        }

        public override bool PcSearchComplete()
        {
            return SearchComplete(By.CssSelector("div[aria-label*='PC search:']"));
        }

        public override bool MobileSearchComplete()
        {
            return SearchComplete(By.CssSelector("div[aria-label*='Mobile search:']"));
        }

        private bool SearchComplete(By rootElement)
        {
            var tallyElement = Driver.WaitUntilElementIsAvailable(rootElement, throwOnTimeout: false);
            if (tallyElement == null)
            {
                return false;
            }

            var label = tallyElement.GetAttribute("aria-label");

            if (string.IsNullOrWhiteSpace(label))
            {
                return false;
            }

            // the label is expected to be in the form: "PC Search: 10/90"

            var parts = label.Split(':');
            if (parts.Length != 2)
            {
                return false;
            }

            var tally = parts[1].Trim();

            var scores = tally.Split('/');

            if (scores.Length != 2) ;

            if (!int.TryParse(scores[0], out var myScore))
            {
                myScore = 0;
            }

            if (!int.TryParse(scores[1], out var total))
            {
                total = 0;
            }

            Log.Info("Completed {tally} of {total}", myScore, total);
            return myScore >= total;
        }
    }
}