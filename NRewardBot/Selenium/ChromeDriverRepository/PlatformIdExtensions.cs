using System;

namespace NRewardBot.Selenium.ChromeDriverRepository
{
    public static class PlatformIdExtensions
    {
        public static string ToPlatform(this PlatformID value)
        {
            // this is not great code.  Quick & dirty code :-/
            switch (value)
            {
                case PlatformID.MacOSX:
                    return Platform.MacArm64;
                case PlatformID.Unix:
                    return Platform.Linux64;
                case PlatformID.Win32NT:
                case PlatformID.Win32S:
                case PlatformID.Win32Windows:
                    return Platform.Win64;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, null);
            }
        }
    }
}