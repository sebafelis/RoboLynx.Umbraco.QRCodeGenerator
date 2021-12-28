using RoboLynx.Umbraco.QRCodeGenerator.Exceptions;
using System;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources
{
    public class AbsoluteUrlSource : QRCodeSource
    {
        private readonly IPublishedContent _content;
        private readonly string _culture;

        public AbsoluteUrlSource(IPublishedContent content, string culture) : base()
        {
            if (content.ItemType == PublishedItemType.Member || 
                content.ItemType == PublishedItemType.Element || 
                content.ItemType == PublishedItemType.Unknown)
            {
                throw new InvalidSettingQRCodeGeneratorException("codeSource", $"Absolute URL source does not support {content.ItemType}");
            }

            _content = content;
            _culture = culture;
        }

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
