using System.Collections.Generic;
using System.Linq;
using NRewardBot.Selenium.Elements;
using OpenQA.Selenium;

namespace NRewardBot.Selenium.Page
{
    internal class RewardDashboardPage : PageBase
    {
        private static readonly NLog.ILogger Log = NLog.LogManager.GetCurrentClassLogger();

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

        public RewardDashboardPage SignInIfRequired()
        {
            Log.Info("Checking for signin page");
            // A Sign in page can sometimes appear (even though we're signed in!)
            var signInAction = this.Driver.WaitUntilElementIsAvailable(By.Id("raf-signin-link-id"), throwOnTimeout: false);
            if (signInAction == null)
            {
                var actions = this.Driver.FindElements(By.CssSelector(".c-call-to-action"));

                foreach (var act in actions)
                {
                    Log.Info(() => $"Action: {act.Text}");
                }

                 signInAction = actions.FirstOrDefault(a => a.Text.Contains("SIGN IN"));
            }

            if (signInAction != null)
            {
                Log.Info("Signin is required...");
                signInAction.Click();
                Log.Info("Signed in");
                this.Driver.DoWait();
            }
            return this;
        }
        public IReadOnlyCollection<OfferLinkElement> GetOpenOffers()
        {
            var doLinq = false;
            if (doLinq)
            {
                var pendingOfferIcons = this.Driver.FindElements(By.XPath("//span[contains(@class, \"mee-icon-AddMedium\")]"));
                var parentElements = pendingOfferIcons.Select(poi => poi.FindElement(By.XPath("..//..//..//..//..")));
                var offerLinks = parentElements.Select(pe => pe.FindElement(By.XPath("div[contains(@class, \"actionLink\")]//descendant::span")));

                return offerLinks.Select(l => new OfferLinkElement(this.Driver, l)).ToArray();
            }

            var elements = new List<OfferLinkElement>();

            var pois = this.Driver.FindElements(By.XPath("//span[contains(@class, \"mee-icon-AddMedium\")]"));
            foreach (var poi in pois)
            {
                var pe = poi.FindElement(By.XPath("..//..//..//.."));
                var actionLink = pe.FindElement(By.XPath("div[contains(@class, \"actionLink\")]"));
                var offerLink = actionLink.FindElement(By.XPath("descendant::span"));

                elements.Add(new OfferLinkElement(this.Driver, offerLink));
            }

            return elements;
        }

        public void SwitchTo()
        {
            this.Driver.SwitchTo().Window(this._windowHandle);
        }
    }
}