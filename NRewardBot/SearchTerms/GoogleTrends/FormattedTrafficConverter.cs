using System;
using Newtonsoft.Json;

namespace NRewardBot.SearchTerms.GoogleTrends
{
    internal class FormattedTrafficConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(FormattedTraffic) || t == typeof(FormattedTraffic?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "100K+":
                    return FormattedTraffic.The100K;
                case "200K+":
                    return FormattedTraffic.The200K;
                case "500K+":
                    return FormattedTraffic.The500K;
                case "50K+":
                    return FormattedTraffic.The50K;
                case "20K+":
                    return FormattedTraffic.The20K;
            }
            throw new Exception("Cannot unmarshal type FormattedTraffic");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (FormattedTraffic)untypedValue;
            switch (value)
            {
                case FormattedTraffic.The100K:
                    serializer.Serialize(writer, "100K+");
                    return;
                case FormattedTraffic.The200K:
                    serializer.Serialize(writer, "200K+");
                    return;
                case FormattedTraffic.The500K:
                    serializer.Serialize(writer, "500K+");
                    return;
                case FormattedTraffic.The50K:
                    serializer.Serialize(writer, "50K+");
                    return;
            }
            throw new Exception("Cannot marshal type FormattedTraffic");
        }

        public static readonly FormattedTrafficConverter Singleton = new FormattedTrafficConverter();
    }
}
