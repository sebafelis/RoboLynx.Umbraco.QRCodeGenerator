using Newtonsoft.Json;
using System;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources
{
    public class AbsoluteUrlSource : QRCodeSource
    {
        public AbsoluteUrlSource(ILocalizedTextService localizedTextService) : base(localizedTextService)
        {

        }

        public override string Id => "AbsoluteUrl";

        public override T GetValue<T>(int index, string key, IPublishedContent content, string sourceSettings)
        {
            if (content is null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            var url = content.Url(null, UrlMode.Absolute);

            return (T)Convert.ChangeType(url, typeof(T));
        }
    }
}
