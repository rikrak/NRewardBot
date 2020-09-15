using System;
using Newtonsoft.Json;

namespace NRewardBot.SearchTerms.GoogleTrends
{
    public partial class Default
    {
        [JsonProperty("trendingSearchesDays")]
        public TrendingSearchesDay[] TrendingSearchesDays { get; set; }

        [JsonProperty("endDateForNextRequest")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long EndDateForNextRequest { get; set; }

        [JsonProperty("rssFeedPageUrl")]
        public Uri RssFeedPageUrl { get; set; }
    }
}