using System;
using NRewardBot.Selenium.Elements;
using OpenQA.Selenium;

namespace NRewardBot.Selenium.Page
{
    public abstract class OfferPageBase : PageBase, IOfferPage
    {
        protected OfferPageBase(IWebDriver driver) : base(driver)
        {
        }

        public abstract IOfferPage CompleteOffer();

        public IOfferPage AcceptCookies()
        {
            var cookieButton = this.Driver.WaitUntilElementIsDisplayed(By.Id("bnp_btn_accept"), throwOnTimeout: false, timeout: TimeSpan.FromSeconds(1));
            cookieButton?.Click();
            return this;
        }

        public void Close()
        {
            this.Driver.Close();
        }
    }
}