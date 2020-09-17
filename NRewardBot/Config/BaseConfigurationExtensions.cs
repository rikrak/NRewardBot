using System;

namespace NRewardBot.Config
{
    internal static class BaseConfigurationExtensions
    {
        public static void ApplyTo(this BaseConfiguration baseConfig, Configuration config)
        {
            if (baseConfig == null) throw new ArgumentNullException(nameof(baseConfig));
            if (config == null) throw new ArgumentNullException(nameof(config));

            config.Mobile = baseConfig.Mobile;
            config.Desktop = baseConfig.Desktop;
            config.Quiz = baseConfig.Quiz;
            config.Email = baseConfig.Email;
            config.Headless = baseConfig.Headless;

            config.SeleniumUrl = baseConfig.SeleniumUrl;
            config.DriverLocation = baseConfig.DriverLocation;
        }
    }
}