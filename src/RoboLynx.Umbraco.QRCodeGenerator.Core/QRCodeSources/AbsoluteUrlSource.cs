using System;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources
{
    public class AbsoluteUrlSource : QRCodeSource
    {
        private readonly IPublishedContent _content;
        private readonly string _culture;

        public AbsoluteUrlSource(ILocalizedTextService localizedTextService, IPublishedContent content, string culture) : base(localizedTextService)
        {
            _content = content;
            _culture = culture;
        }

        public override string Id => "AbsoluteUrl";

        public override T GetValue<T>(int index, string key)
        {
            if (_content is null)
            {
                throw new ArgumentNullException(nameof(_content));
            }

            var url = _content.Url(_culture, UrlMode.Absolute);

            return (T)Convert.ChangeType(url, typeof(T));
        }
    }
}
