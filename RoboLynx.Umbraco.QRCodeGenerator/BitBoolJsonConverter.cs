using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboLynx.Umbraco.QRCodeGenerator
{
    public class BitBoolJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(bool)) || objectType.Equals(typeof(bool?));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            switch (reader.Value)
            {
                case "1":
                case 1:
                case true:
                case "true":
                    return true;
                case "0":
                case 0:
                case false:
                case "false":
                default: 
                    return false;
            }
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
