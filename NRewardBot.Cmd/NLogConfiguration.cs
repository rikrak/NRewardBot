using Microsoft.Extensions.Configuration;
using NLog;
using NLog.Extensions.Logging;

namespace NRewardBot.Cmd
{
    internal static class NLogConfiguration
    {
        public static void Bootstrap()
        {
            var configBuilder = new ConfigurationBuilder();
            var config = configBuilder
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            LogManager.Configuration = new NLogLoggingConfiguration(config.GetSection("NLog"));
        }

    }
}