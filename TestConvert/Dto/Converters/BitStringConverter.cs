using System;
using System.Text;
using Newtonsoft.Json;

namespace TestConvert.Dto.Converters
{
    internal class BitStringConverter : JsonConverter
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
            if (reader.Value != null)
            {
                String[] arr = ((String)reader.Value).Split('-');
                byte[] bytes = new byte[arr.Length];
                for (int i = 0; i < arr.Length; i++)
                {
                    bytes[i] = Convert.ToByte(arr[i], 16);
                }

                string text = Encoding.UTF8.GetString(bytes);
                return text;
            }

            return String.Empty;
        }
    }
}
