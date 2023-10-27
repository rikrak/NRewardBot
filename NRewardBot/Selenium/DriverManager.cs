using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NRewardBot.Config;
using NRewardBot.Selenium.ChromeDriverRepository;

namespace NRewardBot.Selenium
{
    public interface IDriverManager
    {
        Task<string> GetLatestDriver();
    }

    public class DriverManager : IDriverManager
    {
        #region Logger
        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();
        #endregion

        private readonly ISeleniumConfiguration _config;

        public DriverManager(ISeleniumConfiguration config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        private Task ClearOldDriver()
        {
            Log.Info("Removing old chrome driver");
            var folder = GetDriverFolder();
            var driverFilePath = Path.Combine(folder, "chromedriver.exe");

            if (File.Exists(driverFilePath))
            {
                File.Delete(driverFilePath);
            }

            return Task.CompletedTask;
        }

        private string GetCurrentVersion()
        {
            var unzipLocation = GetDriverFolder();
            var versionFilePath = Path.Combine(unzipLocation, "chromedriver.version.txt");
            if (File.Exists(versionFilePath))
            {
                return File.ReadAllText(versionFilePath);
            }

            return null;
        }

        public async Task<string> GetLatestDriver()
        {
            Log.Info("Getting the latest Chrome Driver version from {url}", _config.SeleniumUrl);
            string chromeDriverDownloadJsonData;
            string currentVersion = GetCurrentVersion();
            var unzipLocation = GetDriverFolder();

            using (var client = new HttpClient())
            {
                var versionUrl = _config.SeleniumUrl;
                client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

                chromeDriverDownloadJsonData = await client.GetStringAsync(versionUrl);
            }

            var chromeDriverDownloadData = JsonConvert.DeserializeObject<ChromeDriverDetails>(chromeDriverDownloadJsonData);
            var latestVersion = chromeDriverDownloadData.Channels.Stable.Version;
            if (string.Equals(latestVersion, currentVersion))
            {
                var driverFile = Directory.GetFiles(unzipLocation, "chromedriver.exe").FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(driverFile))
                {
                    Log.Info("latest version of the driver already installed");
                    return driverFile;
                }
            }

            await ClearOldDriver();

            var stableChannel = chromeDriverDownloadData.Channels.Stable;
            var platform = Environment.OSVersion.Platform.ToPlatform();
            var url = stableChannel.Downloads.ChromeDriver.First(cd => cd.Platform == platform).Url;
            using (var client = new HttpClient())
            {
                Log.Info("Getting Chrome Driver version {version} from {url}", latestVersion, url);

                string driverFilePath;
                using (var driverStream = await client.GetStreamAsync(url))
                {
                    using (var z = new ZipArchive(driverStream))
                    {
                        var driverEntry = z.Entries.First(e => e.FullName.ToLower().Contains("chromedriver.exe"));
                        driverFilePath = Path.Combine(unzipLocation, Path.GetFileName(driverEntry.FullName));

                        using (var fs = File.OpenWrite(driverFilePath))
                        {
                            await driverEntry.Open().CopyToAsync(fs);
                            await fs.FlushAsync();
                        }
                    }
                }

                // write a file that contains version information for debug purposes
                var versionFilePath = Path.Combine(unzipLocation, "chromedriver.version.txt");
                if (File.Exists(versionFilePath))
                {
                    File.Delete(versionFilePath);
                }
                File.WriteAllText(versionFilePath, latestVersion);
                Log.Info("Chrome Driver version {version} written to {location}", latestVersion, unzipLocation);

                return driverFilePath;
            }
        }

        private string GetDriverFolder()
        {
            var location = this._config.DriverLocation;
            if (!Path.IsPathRooted(location))
            {
                location = Path.Combine(Environment.CurrentDirectory, location);
            }

            if (!Directory.Exists(location))
            {
                Directory.CreateDirectory(location);
            }

            return location;
        }
    }
}
