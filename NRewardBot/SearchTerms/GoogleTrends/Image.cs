using System;
using Newtonsoft.Json;

namespace NRewardBot.SearchTerms.GoogleTrends
{
    public partial class Image
    {
        [JsonProperty("newsUrl")]
        public Uri NewsUrl { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("imageUrl")]
        public Uri ImageUrl { get; set; }
    }
}