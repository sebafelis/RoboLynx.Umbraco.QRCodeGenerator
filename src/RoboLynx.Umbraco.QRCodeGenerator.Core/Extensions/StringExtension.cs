using Newtonsoft.Json;
using System;
using System.Linq;

namespace RoboLynx.Umbraco.QRCodeGenerator
{
    public static class StringExtension
    {
        public static bool TryParseJson<T>(this string @this, out T result)
        {
            bool success = true;
            var settings = new JsonSerializerSettings
            {
                Error = (sender, args) => { success = false; args.ErrorContext.Handled = true; },
                MissingMemberHandling = MissingMemberHandling.Error
            };
            result = JsonConvert.DeserializeObject<T>(@this, settings);
            return success;
        }

        public static bool StringToBoolean(this string @string, bool bDefault)
        {
            string[] BooleanStringOff = { "0" };

            if (string.IsNullOrEmpty(@string))
                return bDefault;
            else if (BooleanStringOff.Contains(@string, StringComparer.InvariantCultureIgnoreCase))
                return false;

            if (!bool.TryParse(@string, out bool result))
                result = true;

            return result;
        }
    }
}