using System;
using System.Threading.Tasks;
using NRewardBot.Config;
using NRewardBot.Selenium.Page;

namespace NRewardBot.Selenium
{
    public class RewardScenario
    {
        private readonly WebDriverFactory _driverFactory;
        private readonly ICredentials _credentials;

        public RewardScenario(WebDriverFactory driverFactory, ICredentials credentials)
        {
            _driverFactory = driverFactory ?? throw new ArgumentNullException(nameof(driverFactory));
            _credentials = credentials ?? throw new ArgumentNullException(nameof(credentials));
        }

        public async Task Test()
        {
            using (var driver = await _driverFactory.GetDriver("Mozilla/5.0 (Windows Phone 10.0; Android 4.2.1; WebView/3.0)"))
            {
                LiveLoginPage login = LiveLoginPage.NavigateTo(driver);
                driver.DoWait(2);
                login.WithUsername(this._credentials.Username)
                    .PressNext()
                    .WithPassword(this._credentials.Password)
                    .PressSubmit();
                driver.DoWait(10); // wait for MFA to be completed

                driver.Close();
            }
        }
    }
}
