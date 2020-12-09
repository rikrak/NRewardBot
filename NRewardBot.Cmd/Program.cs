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
            try
            {
                Log.Info("Started");
                NLogConfiguration.Bootstrap();
                var configFactory = new ConfigurationFactory();
                var config = configFactory.GetConfiguration(args);

                var driver = new DriverManager(config);

                var webDriverFactory = new WebDriverFactory(config, driver);

                if (config.Quiz)
                {
                    var rewardScenario = new RewardScenario(webDriverFactory, config);
                    await rewardScenario.DailyOffers();
                }

                if (config.Desktop || config.Mobile)
                {
                    var searchTermProvider = new SearchTermProvider();
                    var searchScenario = new SearchScenario(webDriverFactory, config, searchTermProvider, config);
                    await searchScenario.DoSearches();
                }

                Log.Info("All Done");
            }
            catch (Exception e)
            {
                Log.Error(e);
            }

            // wait for user to acknowledge
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }
    }
}
