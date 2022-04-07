using RoboLynx.Umbraco.QRCodeGenerator.Models;
using System.IO;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace RoboLynx.Umbraco.QRCodeGenerator.Frontend
{
    public class QRCode
    {
        private readonly IQRCodeService _qrCodeService;
        private QRCodeSettings _defaultSettings;

        internal QRCode(IQRCodeService qrCodeService, IPublishedContent publishedContent, string propertyAlias, string cacheName)
        {
            _qrCodeService = qrCodeService;
            PublishedContent = publishedContent;
            PropertyAlias = propertyAlias;
            CacheName = cacheName;
        }

        public QRCodeSettings DefaultSettings
        {
            get
            {
                if (_defaultSettings is null)
                {
                    _defaultSettings = _qrCodeService.GetDefaultSettings(PublishedContent, PropertyAlias);
                }
                return _defaultSettings;
            }
        }

        public Stream Stream(string culture, QRCodeSettings settings)
        {
            return _qrCodeService.GetStream(PublishedContent, PropertyAlias, culture, settings, CacheName);
        }

        public Stream Stream(string culture)
        {
            return _qrCodeService.GetStream(PublishedContent, PropertyAlias, culture, DefaultSettings, CacheName);
        }

        public Stream Stream()
        {
            return _qrCodeService.GetStream(PublishedContent, PropertyAlias, PublishedContent.GetCultureFromDomains(), DefaultSettings, CacheName);
        }

        public IPublishedContent PublishedContent { get; }
        public string PropertyAlias { get; }
        public string CacheName { get; }
    }
}