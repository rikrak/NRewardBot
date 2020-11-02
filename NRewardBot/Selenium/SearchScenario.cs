using System;
using System.Linq;
using System.Threading.Tasks;
using NRewardBot.Config;
using NRewardBot.SearchTerms.GoogleTrends;
using NRewardBot.Selenium.Page;
using OpenQA.Selenium;

namespace NRewardBot.Selenium
{
    public class SearchScenario
    {
        #region Logger
        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();
        #endregion

        private readonly WebDriverFactory _driverFactory;
        private readonly ICredentials _credentials;
        private readonly SearchTermProvider _searchTermProvider;
        private readonly IConfiguration _configuration;

        #region Constructor

        public SearchScenario(WebDriverFactory driverFactory, ICredentials credentials, SearchTermProvider searchTermProvider, IConfiguration configuration)
        {
            _driverFactory = driverFactory ?? throw new ArgumentNullException(nameof(driverFactory));
            _credentials = credentials ?? throw new ArgumentNullException(nameof(credentials));
            _searchTermProvider = searchTermProvider ?? throw new ArgumentNullException(nameof(searchTermProvider));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        #endregion

        public async Task DoSearches()
        {
            if (this._configuration.Desktop)
            {
                await DoSearches(UserAgent.Desktop);
            }
            if (this._configuration.Mobile)
            {
                await DoSearches(UserAgent.Mobile);
            }
        }

        private async Task DoSearches(string userAgent)
        {
            using (var driver = await _driverFactory.GetDriver(userAgent))
            {
                var loginTask = DoLogin(driver);
                var searchTermsTask = _searchTermProvider.GetTerms();

                await Task.WhenAll(loginTask, searchTermsTask);

                Log.Info("Processing Searches");

                var searchTerms = searchTermsTask.Result.ToList();
                searchTerms.Shuffle();
                var searchPage = BingSearchPage.NavigateTo(driver);
                var maxSearches = userAgent == UserAgent.Mobile ? 20 : 30;
                foreach (var searchTerm in searchTerms)
                {
                    searchPage.Search(searchTerm);
                    if (--maxSearches < 0)
                    {
                        break;
                    }
                }

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