using Microsoft.Extensions.Configuration;

namespace NRewardBot.Config
{
    class BaseConfiguration : IConfiguration, ISeleniumConfiguration
    {
        private BaseConfiguration() { }

        public static BaseConfiguration Bootstrap()
        {
            var configBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json");
            var configuration = configBuilder.Build();

            var result = new BaseConfiguration()
            {
                Headless = GetOptionalBool(configuration, "appSettings:headless") ?? false,
                Mobile = GetOptionalBool(configuration, "appSettings:mobile") ?? false,
                Desktop = GetOptionalBool(configuration, "appSettings:desktop") ?? false,
                Quiz = GetOptionalBool(configuration, "appSettings:quiz") ?? false,
                Email = GetOptionalBool(configuration, "appSettings:email") ?? false,
                
                SeleniumUrl = configuration["selenium:driverUrl"],
                DriverLocation = configuration["selenium:driverLocation"],
            };

            return result;
        }

        public bool Headless { get; private set; }
        public bool Mobile   { get; private set; }
        public bool Desktop  { get; private set; }
        public bool Quiz     { get; private set; }
        public bool Email    { get; private set; }

        public string SeleniumUrl { get; private set; }
        public string DriverLocation { get; private set; }


        private static bool? GetOptionalBool(Microsoft.Extensions.Configuration.IConfiguration config, string path)
        {
            var value = config[path];
            if (bool.TryParse(value, out var result))
            {
                return result;
            }

            return null;
        }
    }
}
