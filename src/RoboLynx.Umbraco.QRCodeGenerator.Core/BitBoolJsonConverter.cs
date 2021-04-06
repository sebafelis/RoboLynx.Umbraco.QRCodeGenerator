using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboLynx.Umbraco.QRCodeGenerator
{
    internal class BitBoolJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(bool)) || objectType.Equals(typeof(bool?));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.Value switch
            {
                "1" or 1 or true or "true" => true,
                _ => false,
            };
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is bool boolValue)
            {
                writer.WriteValue(boolValue ? "1" : "0");
            }
        }

        public override bool CanRead => true;

        public override bool CanWrite => true;
    }
}
