using OpenQA.Selenium;

namespace NRewardBot.Selenium.Page
{
    public abstract class OfferPageBase : BingSearchPageBase, IOfferPage
    {
        protected OfferPageBase(IWebDriver driver) : base(driver)
        {
        }

        public abstract IOfferPage CompleteOffer();

        IOfferPage IOfferPage.EnsureLoggedIn() => this.EnsureLoggedIn<OfferPageBase>();

        IOfferPage IOfferPage.AcceptCookies() => this.AcceptCookies<OfferPageBase>();

        public void Close()
        {
            this.Driver.Close();
        }
    }
}