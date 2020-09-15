using Newtonsoft.Json;

namespace NRewardBot.SearchTerms.GoogleTrends
{
    public partial class Title
    {
        [JsonProperty("query")]
        public string Query { get; set; }

        [JsonProperty("exploreLink")]
        public string ExploreLink { get; set; }
    }
}