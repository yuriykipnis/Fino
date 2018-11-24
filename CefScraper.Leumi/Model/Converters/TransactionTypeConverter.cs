using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CefScraper.Leumi.Model.Converters
{
    internal class TransactionTypeConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            TransactionType transactionType = (TransactionType)value;
            var writeVal = transactionType == TransactionType.Expense ? 1 : 0;
            writer.WriteValue(writeVal);
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String)
            {
                String type = serializer.Deserialize(reader, objectType).ToString();
                return type == "Expense" ? TransactionType.Expense : TransactionType.Income;
            }

            return TransactionType.None;
        }
    }
}
