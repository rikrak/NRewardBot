using System;
using System.Threading.Tasks;

namespace NRewardBot.Selenium
{
    public class RewardScenario
    {
        private readonly WebDriverFactory _driverFactory;

        public RewardScenario(WebDriverFactory driverFactory)
        {
            _driverFactory = driverFactory ?? throw new ArgumentNullException(nameof(driverFactory));
        }

        public async Task Test()
        {
            using (var driver = await _driverFactory.GetDriver("Mozilla/5.0 (Windows Phone 10.0; Android 4.2.1; WebView/3.0)"))
            {
                driver.Url = "https://login.live.com";
                await Task.Delay(2000);
                driver.Close();
            }
        }
    }
}
