using OpenQA.Selenium;

namespace NRewardBot.Selenium.Page
{
    public class ExploreDailyPage : OfferPageBase, IOfferPage
    {
        #region Logger
        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();
        #endregion

        protected IWebElement PageElement => this.Driver.FindElement(By.TagName("html"));

        public ExploreDailyPage(IWebDriver driver) : base(driver)
        {
        }

        public override void CompleteOffer()
        {
            Log.Info("Completing an explore page");
            for (int i = 0; i < 3; i++)
            {
                this.PageElement.SendKeys(Keys.End);
                this.PageElement.SendKeys(Keys.Home);
            }
        }
    }
}