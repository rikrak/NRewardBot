using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using NRewardBot.Config;

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
            string latestVersion;
            string currentVersion = GetCurrentVersion();
            var unzipLocation = GetDriverFolder();

            using (var client = new HttpClient())
            {
                var versionUrl = _config.SeleniumUrl;
                client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

                latestVersion = await client.GetStringAsync(versionUrl);
            }

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

            using (var client = new HttpClient())
            {
                string url;
                switch (Environment.OSVersion.Platform)
                {
                    case PlatformID.MacOSX:
                        url = $"https://chromedriver.storage.googleapis.com/{latestVersion}/chromedriver_mac64.zip";
                        break;
                    case PlatformID.Unix:
                        url = "https://chromedriver.storage.googleapis.com/{}/chromedriver_linux64.zip";
                        break;
                    case PlatformID.Win32NT:
                    case PlatformID.Win32S:
                    case PlatformID.Win32Windows:
                        url = $"https://chromedriver.storage.googleapis.com/{latestVersion}/chromedriver_win32.zip";
                        break;
                    default:
                        throw new NotSupportedException($"{Environment.OSVersion.Platform} is not supported");
                }
                Log.Info("Getting Chrome Driver version {version} from {url}", latestVersion, url);

                string driverFilePath;
                using (var driverStream = await client.GetStreamAsync(url))
                {
                    using (var z = new ZipArchive(driverStream))
                    {
                        var driverEntry = z.Entries.First(e => e.FullName.ToLower().Contains("chromedriver"));
                        driverFilePath = Path.Combine(unzipLocation, driverEntry.FullName);

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
