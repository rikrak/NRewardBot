using System.Collections.Generic;
using System.Linq;
using NRewardBot.Selenium.Elements;
using OpenQA.Selenium;

namespace NRewardBot.Selenium.Page
{
    internal class RewardDashboardPage : PageBase
    {
        private readonly string _windowHandle;
        private const string PageUrl = "https://account.microsoft.com/rewards/dashboard";

        #region Constructor
        public RewardDashboardPage(IWebDriver driver) : base(driver)
        {
            this._windowHandle = driver.CurrentWindowHandle;

        }
        #endregion


        public static RewardDashboardPage NavigateTo(IWebDriver driver)
        {
            driver.Navigate().GoToUrl(PageUrl);

            return new RewardDashboardPage(driver);
        }

        public IReadOnlyCollection<OfferLinkElement> GetOpenOffers()
        {
            var pendingOfferIcons = this.Driver.FindElements(By.XPath("//span[contains(@class, \"mee-icon-AddMedium\")]"));
            var parentElements = pendingOfferIcons.Select(poi => poi.FindElement(By.XPath("..//..//..//..")));
            var offerLinks = parentElements.Select(pe => pe.FindElement(By.XPath("div[contains(@class, \"actionLink\")]//descendant::a")));

            return offerLinks.Select(l => new OfferLinkElement(this.Driver, l)).ToArray();
        }

        public void SwitchTo()
        {
            this.Driver.SwitchTo().Window(this._windowHandle);
        }
    }
}