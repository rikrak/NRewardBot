using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NRewardBot.Config;
using NRewardBot.SearchTerms.GoogleTrends;

namespace NRewardBot.Selenium
{
    public interface IDriverManager
    {
        Task<string> GetLatestDriver();
    }

    public class DriverManager : IDriverManager
    {
        private readonly ISeleniumConfiguration _config;

        public DriverManager(ISeleniumConfiguration config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        private Task ClearOldDriver()
        {
            var folder = GetDriverFolder();
            var driverFilePath = Path.Combine(folder, "chromedriver.exe");

            if (File.Exists(driverFilePath))
            {
                File.Delete(driverFilePath);
            }

            return Task.CompletedTask;
        }

        public async Task<string> GetLatestDriver()
        {
            string latestVersion;

            await ClearOldDriver();
            using (var client = new HttpClient())
            {
                var versionUrl = _config.SeleniumUrl;
                client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

                latestVersion = await client.GetStringAsync(versionUrl);
            }

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

                var unzipLocation = GetDriverFolder();
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
