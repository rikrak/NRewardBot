using System;
using NRewardBot.Selenium.Elements;
using OpenQA.Selenium;

namespace NRewardBot.Selenium.Page
{
    internal abstract class RewardStatusPage : PageBase
    {
        #region Logger

        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();

        #endregion

        private static readonly Random Random = new Random(DateTime.Now.Millisecond);
        private const string PageUrl = "http://www.bing.com/rewardsapp/bepflyoutpage?style=chromeextension";

        #region COnstructor

        protected RewardStatusPage(IWebDriver driver) : base(driver)
        {
        }
        
        #endregion

        public static RewardStatusPage NavigateTo(IWebDriver driver)
        {
            driver.Navigate().GoToUrl(PageUrl);

            var v1ElementMarker = driver.WaitUntilElementIsAvailable(By.Id(RewardStatusPageV1.CreditsElementId), throwOnTimeout: false, TimeSpan.FromSeconds(4));
            RewardStatusPage rewardStatusPage = null;
            if (v1ElementMarker != null)
            {
                rewardStatusPage = new RewardStatusPageV1(driver);
                driver.WaitUntilElementIsDisplayed(By.ClassName("pcsearch"), throwOnTimeout: false, TimeSpan.FromSeconds(4));
            }

            if (rewardStatusPage == null)
            {
                var v2ElementMarker = driver.WaitUntilElementIsAvailable(By.Id(RewardStatusPageV2.PageElementId), throwOnTimeout: false, TimeSpan.FromSeconds(4));

                if (v2ElementMarker != null)
                {
                    rewardStatusPage = new RewardStatusPageV2(driver);
                }
            }

            return rewardStatusPage ?? new NullRewardStatusPage(driver);
        }

        public abstract bool IsLevelOne();

        public abstract bool AllSearchComplete();

        public abstract bool PcSearchComplete();

        public abstract bool MobileSearchComplete();


        //public int GetPcSearchTally()
        //{
        //    return this.GetSearchTally(By.ClassName("pcsearch"));
        //}


        //public int GetMobileSearchTally()
        //{
        //    return this.GetSearchTally(By.ClassName("mobilesearch"));
        //}

        //public int GetPcSearchTotal()
        //{
        //    return GetSearchTotal(By.ClassName("pcsearch"));
        //}

        //public int GetMobileTotal()
        //{
        //    return GetSearchTotal(By.ClassName("mobilesearch"));
        //}


    }
}