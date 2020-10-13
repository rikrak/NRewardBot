using OpenQA.Selenium;

namespace NRewardBot.Selenium.Page
{
    public class ExploreDailyPage : OfferPageBase, IOfferPage
    {
        protected IWebElement PageElement => this.Driver.FindElement(By.TagName("html"));

        public ExploreDailyPage(IWebDriver driver) : base(driver)
        {
        }

        public override void CompleteOffer()
        {
            for (int i = 0; i < 3; i++)
            {
                this.PageElement.SendKeys(Keys.End);
                this.PageElement.SendKeys(Keys.Home);
            }
        }
    }
}