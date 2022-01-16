using RoboLynx.Umbraco.QRCodeGenerator.Exceptions;
using System;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Routing;
using Umbraco.Extensions;
using UmbracoCoreConstants = Umbraco.Cms.Core.Constants;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources
{
    public class AbsoluteUrlSource : QRCodeSource
    {
        private readonly IPublishedUrlProvider _publishedUrlProvider;
        private readonly IPublishedContent _content;
        private readonly string _culture;

        public AbsoluteUrlSource(IPublishedUrlProvider publishedUrlProvider, IPublishedContent content, string culture) : base()
        {
            if (content.ContentType.ItemType == PublishedItemType.Member || 
                content.ContentType.ItemType == PublishedItemType.Element || 
                content.ContentType.ItemType == PublishedItemType.Unknown)
            {
                throw new InvalidSettingQRCodeGeneratorException("codeSource", $"Absolute URL source does not support {content.ItemType}");
            }
            _publishedUrlProvider = publishedUrlProvider;
            _content = content;
            _culture = culture;
        }

        public override T GetValue<T>(int index, string key)
        {
            if (_content is null)
            {
                throw new ArgumentNullException(nameof(_content));
            }

            if (_publishedUrlProvider == null)
                throw new InvalidOperationException("Cannot resolve a Url when IPublishedUrlProvider is not register.");

            string url;
            switch (_content.ContentType.ItemType)
            {
                case PublishedItemType.Content:
                    url = _publishedUrlProvider.GetUrl(_content, UrlMode.Absolute, _culture);
                    break;

                case PublishedItemType.Media:
                    url = _publishedUrlProvider.GetMediaUrl(_content, UrlMode.Absolute, _culture, UmbracoCoreConstants.Conventions.Media.File);
                    break;

                default:
                    throw new NotSupportedException();
            }

            return (T)Convert.ChangeType(url, typeof(T));
        }
    }
}
