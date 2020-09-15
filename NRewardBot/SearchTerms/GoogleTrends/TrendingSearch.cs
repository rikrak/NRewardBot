using System;
using Newtonsoft.Json;

namespace NRewardBot.SearchTerms.GoogleTrends
{
    public partial class TrendingSearch
    {
        [JsonProperty("title")]
        public Title Title { get; set; }

        [JsonProperty("formattedTraffic")]
        public string FormattedTraffic { get; set; }

        [JsonProperty("relatedQueries")]
        public Title[] RelatedQueries { get; set; }

        [JsonProperty("image")]
        public Image Image { get; set; }

        [JsonProperty("articles")]
        public Article[] Articles { get; set; }

        [JsonProperty("shareUrl")]
        public Uri ShareUrl { get; set; }
    }
}