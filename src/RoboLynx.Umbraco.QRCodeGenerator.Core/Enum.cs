using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace RoboLynx.Umbraco.QRCodeGenerator
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ECCLevel
    {
        L = 0,
        M = 1,
        Q = 2,
        H = 3
    }
}