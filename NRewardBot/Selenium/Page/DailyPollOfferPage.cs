using System;
using OpenQA.Selenium;

namespace NRewardBot.Selenium.Page
{
    public class DailyPollOfferPage : OfferPageBase, IOfferPage
    {
        #region Logger
        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();
        #endregion

        private static readonly string[] OptionIds = new[] { "btoption0", "btoption1"};
        private static readonly Random Randomiser = new Random(DateTime.Now.Millisecond);

        public DailyPollOfferPage(IWebDriver driver) : base(driver)
        {
        }

        public static bool IsDailyPollPage(IWebDriver driver)
        {
            try
            {
                return driver.FindElement(By.Id(OptionIds[0])) != null;
            }
            catch (NoSuchElementException e)
            {
                return false;
            }
        }

        public override IOfferPage CompleteOffer()
        {
            Log.Info("Completing the daily poll");
            var idToFind = OptionIds[Randomiser.Next(OptionIds.Length)];
            var elementLocator = By.Id(idToFind);

            var optionElement = this.Driver.WaitUntilElementIsDisplayed(elementLocator);
            optionElement.Click();
            this.Driver.DoWait(3);
            return this;
        }
    }
}