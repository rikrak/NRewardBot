using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using NRewardBot.Config;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace NRewardBot.Selenium
{
    public class WebDriverFactory
    {
        private readonly IConfiguration _configuration;
        private readonly IDriverManager _driverManager;

        public WebDriverFactory(IConfiguration configuration, IDriverManager driverManager)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _driverManager = driverManager ?? throw new ArgumentNullException(nameof(driverManager));
        }

        public async Task<IWebDriver> GetDriver(string userAgent)
        {
            var driverFilePath = await _driverManager.GetLatestDriver();

            var service = ChromeDriverService.CreateDefaultService(Path.GetDirectoryName(driverFilePath));

            var browserOptions = new ChromeOptions();
            
            browserOptions.AddArgument($"user-agent={userAgent}");
            browserOptions.AddArgument("--disable-webgl");
            browserOptions.AddArgument("--no-sandbox");
            browserOptions.AddArgument("--disable-extensions");
            browserOptions.AddArgument("--disable-dev-shm-usage");
            if (this._configuration.Headless)
            {
                browserOptions.AddArgument("--headless");
            }
            //options.add_experimental_option('w3c', False)

            //prefs = {
            //    "profile.default_content_setting_values.geolocation" : 2, "profile.default_content_setting_values.notifications": 2
            //}

            //options.add_experimental_option("prefs", prefs)

            return new ChromeDriver(service, browserOptions);
        }
    }
}
