using System;
using System.Threading.Tasks;
using NLog.Config;
using NRewardBot.Config;
using NRewardBot.SearchTerms.GoogleTrends;
using NRewardBot.Selenium;

namespace NRewardBot.Cmd
{
    class Program
    {
        #region Logger
        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();
        #endregion

        static async Task Main(string[] args)
        {
            Log.Info("Started");
            NLogConfiguration.Bootstrap();
            var configFactory = new ConfigurationFactory();
            var config = configFactory.GetConfiguration(args);

            var driver = new DriverManager(config);

            var webDriverFactory = new WebDriverFactory(config, driver);
            var searchTermProvider = new SearchTermProvider();
            var scenario = new RewardScenario(webDriverFactory, config,searchTermProvider);

            await scenario.DailyOffersAndSearches();
            Log.Info("All Done");
        }
    }
}
