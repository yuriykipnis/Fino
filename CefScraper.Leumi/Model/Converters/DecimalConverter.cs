using System;
using Newtonsoft.Json;

namespace CefScraper.Leumi.Model.Converters
{
    internal class DecimalConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Decimal date = (Decimal)value;
            writer.WriteValue(date);
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String)
            {
                String number = serializer.Deserialize(reader, objectType).ToString();
                return Convert.ToDecimal(CommonScraper.ToUtf8(number));
            }

            return TransactionType.None;
        }
    }
}
