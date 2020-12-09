using OpenQA.Selenium;

namespace NRewardBot.Selenium.Page
{
    internal class NullRewardStatusPage : RewardStatusPage
    {
        public NullRewardStatusPage(IWebDriver driver) : base(driver)
        {
        }

        public override bool IsLevelOne()
        {
            return false;
        }

        public override bool AllSearchComplete()
        {
            return false;
        }

        public override bool PcSearchComplete()
        {
            return false;
        }

        public override bool MobileSearchComplete()
        {
            return false;
        }
    }
}