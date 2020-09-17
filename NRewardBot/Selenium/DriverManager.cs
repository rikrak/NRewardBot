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
    public class DriverManager
    {
        private readonly ISeleniumConfiguration _config;

        public DriverManager(ISeleniumConfiguration config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public async Task GetLatestDriver()
        {
            string latestVersion;
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
                using (var driverStream = await client.GetStreamAsync(url))
                {
                    using (var z = new ZipArchive(driverStream))
                    {
                        foreach (var entry in z.Entries)
                        {
                            using (var fs = File.OpenWrite(Path.Combine(unzipLocation, entry.FullName)))
                            {
                                await entry.Open().CopyToAsync(fs);
                                await fs.FlushAsync();
                            }
                        }
                    }
                }
            
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
