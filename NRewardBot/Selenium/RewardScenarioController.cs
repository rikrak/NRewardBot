using System;
using System.Linq;
using System.Threading.Tasks;
using NRewardBot.Config;
using NRewardBot.SearchTerms.GoogleTrends;
using NRewardBot.Selenium.Page;
using OpenQA.Selenium;

namespace NRewardBot.Selenium
{
    public class RewardScenario
    {
        #region Logger
        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();
        #endregion

        private readonly WebDriverFactory _driverFactory;
        private readonly ICredentials _credentials;

        #region Constructor

        public RewardScenario(WebDriverFactory driverFactory, ICredentials credentials)
        {
            _driverFactory = driverFactory ?? throw new ArgumentNullException(nameof(driverFactory));
            _credentials = credentials ?? throw new ArgumentNullException(nameof(credentials));
        }

        #endregion

        public async Task DailyOffers()
        {
            using (var driver = await _driverFactory.GetDriver(UserAgent.Desktop))
            {
                await DoLogin(driver);

                Log.Info("Proecessing daily offers");
                var rewardDashboard = RewardDashboardPage.NavigateTo(driver);
                var openOfferLinks = rewardDashboard.GetOpenOffers();
                
                Log.Info("There are {count} offer(s)", openOfferLinks.Count);
                foreach (var link in openOfferLinks)
                {
                    driver.DoWait(3);
                    var offerPage = link.Click();
                    offerPage?.CompleteOffer();
                    offerPage?.Close();


                    rewardDashboard.SwitchTo();
                }
//#here -> line ~593 in https://github.com/LjMario007/Microsoft-Rewards-Bot/blob/master/ms_rewards.py

                driver.Close();
            }
        }


        private Task DoLogin(IWebDriver driver)
        {
            Log.Info("Processing login to Microsoft account");
            var login = LiveLoginPage.NavigateTo(driver);
            driver.DoWait(2);
            login.WithUsername(this._credentials.Username)
                .PressNext()
                .WithPassword(this._credentials.Password)
                .PressSubmit()
                .DoMultiFactorAuth()
                .PressStayLoggedIn();

            return Task.CompletedTask;
        }
    }
}
