using System;
using System.Threading.Tasks;
using NRewardBot.Config;
using NRewardBot.Selenium;

namespace NRewardBot.Cmd
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var configFactory = new ConfigurationFactory();
            var config = configFactory.GetConfiguration(args);

            var driver = new DriverManager(config);

            var webDriverFactory = new WebDriverFactory(config, driver);

            var scenario = new RewardScenario(webDriverFactory, config);

            await scenario.Test();
        }
    }
}
