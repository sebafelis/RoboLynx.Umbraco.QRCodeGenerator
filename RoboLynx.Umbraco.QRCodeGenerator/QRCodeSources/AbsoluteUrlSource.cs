using Newtonsoft.Json;
using System;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources
{
    public class AbsoluteUrlSource : QRCodeSource
    {
        public class AbsoluteUrlSourceSettings
        {

        }

        AbsoluteUrlSourceSettings settings;

        public AbsoluteUrlSource() : base(null)
        {

        }

        public AbsoluteUrlSource(IPublishedContent content, string sourceSettings) : base(content)
        {
            if (!string.IsNullOrEmpty(sourceSettings))
            {
                settings = JsonConvert.DeserializeObject<AbsoluteUrlSourceSettings>(sourceSettings);
            }
        }

        public override T GetValue<T>(int index, string key)
        {
            CheckContent();

            var url = content.UrlAbsolute();

            return (T)Convert.ChangeType(url, typeof(T));
        }
    }
}
