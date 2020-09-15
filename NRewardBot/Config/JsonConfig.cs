using System.Threading.Tasks;
using Newtonsoft.Json;
using NRewardBot.SearchTerms.GoogleTrends;

namespace NRewardBot.Config
{
    internal static class JsonConfig
    {
        public static Task Bootstrap()
        {
            JsonConvert.DefaultSettings = () => Converter.Settings;
            return Task.CompletedTask;
        }
    }
}
