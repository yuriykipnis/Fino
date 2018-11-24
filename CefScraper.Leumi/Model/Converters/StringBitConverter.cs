using System;
using System.Drawing.Imaging;
using System.Text;
using Newtonsoft.Json;

namespace CefScraper.Leumi.Model.Converters
{
    internal class StringBitConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value);
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String)
            {
                if (reader.Value is string text)
                {
                    var bytes = Encoding.UTF8.GetBytes(text);
                    return BitConverter.ToString(bytes);
                }
            }

            return new byte[0];
        }
    }
}
