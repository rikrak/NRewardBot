using NRewardBot.Selenium.Elements;
using OpenQA.Selenium;

namespace NRewardBot.Selenium.Page
{
    public abstract class OfferPageBase : PageBase, IOfferPage
    {
        protected OfferPageBase(IWebDriver driver) : base(driver)
        {
        }

        public abstract void CompleteOffer();

        public void Close()
        {
            this.Driver.Close();
        }
    }
}