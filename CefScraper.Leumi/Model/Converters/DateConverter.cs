using System;
using System.Reflection;
using Newtonsoft.Json;

namespace CefScraper.Leumi.Model.Converters
{
    internal class DateConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            DateTime date = (DateTime)value;
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
                var date = reader.Value.ToString().Split('/');
                return new DateTime(2000 + Convert.ToInt32(date[2]), Convert.ToInt32(date[1]), Convert.ToInt32(date[0]));
            }

            return DateTime.MinValue;
        }
    }
}
