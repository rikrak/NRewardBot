using Newtonsoft.Json;

namespace NRewardBot.SearchTerms.GoogleTrends
{
    public partial class TrendingSearchesDay
    {
        [JsonProperty("date")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Date { get; set; }

        [JsonProperty("formattedDate")]
        public string FormattedDate { get; set; }

        [JsonProperty("trendingSearches")]
        public TrendingSearch[] TrendingSearches { get; set; }
    }
}