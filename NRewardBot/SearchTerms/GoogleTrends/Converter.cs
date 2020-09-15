using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NRewardBot.SearchTerms.GoogleTrends
{
    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                TimeAgoConverter.Singleton,
                FormattedTrafficConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}