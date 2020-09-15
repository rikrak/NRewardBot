using System;
using Newtonsoft.Json;

namespace NRewardBot.SearchTerms.GoogleTrends
{
    public partial class Article
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("timeAgo")]
        public string TimeAgo { get; set; }
        //public TimeAgo TimeAgo { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("image", NullValueHandling = NullValueHandling.Ignore)]
        public Image Image { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("snippet")]
        public string Snippet { get; set; }
    }
}