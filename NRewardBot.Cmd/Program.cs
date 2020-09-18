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

            var scenario = new RewardScenario(new WebDriverFactory(config, driver));

            await scenario.Test();
        }
    }
}
