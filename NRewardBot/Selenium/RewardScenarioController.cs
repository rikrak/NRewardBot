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
        private readonly WebDriverFactory _driverFactory;
        private readonly ICredentials _credentials;
        private readonly SearchTermProvider _searchTermProvider;

        public RewardScenario(WebDriverFactory driverFactory, ICredentials credentials, SearchTermProvider searchTermProvider)
        {
            _driverFactory = driverFactory ?? throw new ArgumentNullException(nameof(driverFactory));
            _credentials = credentials ?? throw new ArgumentNullException(nameof(credentials));
            _searchTermProvider = searchTermProvider ?? throw new ArgumentNullException(nameof(searchTermProvider));
        }

        public async Task Test()
        {
            using (var driver = await _driverFactory.GetDriver("Mozilla/5.0 (Windows Phone 10.0; Android 4.2.1; WebView/3.0)"))
            {
                var loginTask = DoLogin(driver);

                var searchTermsTask = _searchTermProvider.GetTerms();

                await Task.WhenAll(loginTask, searchTermsTask);
                var searchTerms = searchTermsTask.Result.ToArray();

                var rewardDashboard = RewardDashboardPage.NavigateTo(driver);
                var openOfferLinks = rewardDashboard.GetOpenOffers();
                foreach (var link in openOfferLinks)
                {
                    driver.DoWait(3);
                    link.Click();
                }
//#here -> line ~593 in https://github.com/LjMario007/Microsoft-Rewards-Bot/blob/master/ms_rewards.py
                driver.Close();
            }
        }

        private Task DoLogin(IWebDriver driver)
        {
            var login = LiveLoginPage.NavigateTo(driver);
            driver.DoWait(2);
            login.WithUsername(this._credentials.Username)
                .PressNext()
                .WithPassword(this._credentials.Password)
                .PressSubmit();
            driver.DoWait(20); // wait for MFA to be completed

            return Task.CompletedTask;
        }
    }
}
