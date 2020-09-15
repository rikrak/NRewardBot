using Newtonsoft.Json;

namespace NRewardBot.SearchTerms.GoogleTrends
{
    public class GoogleTrends
    {
        [JsonProperty("default")]
        public Default Default { get; set; }
    }
}