using System;
using Newtonsoft.Json;

namespace NRewardBot.SearchTerms.GoogleTrends
{
    internal class TimeAgoConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(TimeAgo) || t == typeof(TimeAgo?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "1w ago":
                    return TimeAgo.The1WAgo;
                case "2w ago":
                    return TimeAgo.The2WAgo;
            }
            throw new Exception("Cannot unmarshal type TimeAgo");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (TimeAgo)untypedValue;
            switch (value)
            {
                case TimeAgo.The1WAgo:
                    serializer.Serialize(writer, "1w ago");
                    return;
                case TimeAgo.The2WAgo:
                    serializer.Serialize(writer, "2w ago");
                    return;
            }
            throw new Exception("Cannot marshal type TimeAgo");
        }

        public static readonly TimeAgoConverter Singleton = new TimeAgoConverter();
    }
}