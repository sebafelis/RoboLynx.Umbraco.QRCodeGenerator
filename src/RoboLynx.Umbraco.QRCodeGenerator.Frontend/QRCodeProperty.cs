using RoboLynx.Umbraco.QRCodeGenerator;
using RoboLynx.Umbraco.QRCodeGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Routing;
using Umbraco.Core.Models;
using Umbraco.Core.Models.Entities;
using Umbraco.Core.Models.Membership;
using Umbraco.Core.Models.PublishedContent;

namespace RoboLynx.Umbraco.QRCodeGenerator.Frontend
{
    public class QRCodeProperty
    {
        private readonly UrlHelper _urlHelper;
        private readonly IPublishedContent _publishedContent;
        private readonly string _propertyAlias;
        private readonly IQRCodeBuilder _qrCodeBuilder;

        private QRCodeSettings _defaultSettings;

        public QRCodeProperty(UrlHelper urlHelper, IPublishedContent publishedContent, string propertyAlias, IQRCodeBuilder qrCodeBuilder, string cacheName = Constants.FrontendCacheName)
        {
            _urlHelper = urlHelper;
            _publishedContent = publishedContent;
            _propertyAlias = propertyAlias;
            _qrCodeBuilder = qrCodeBuilder;
        }

        public string Url(string culture, QRCodeSettings settings, UrlMode urlMode = UrlMode.Auto)
        {
            return _urlHelper.Link("default", new { action = "Get", controller = "QRCodePublic" });
        }

        public string DirectUrl(string culture, QRCodeSettings settings, string cacheName)
        {
            var config = _qrCodeBuilder.CreateConfiguration(_publishedContent, _propertyAlias, culture, settings);
            return _qrCodeBuilder.GetUrl(config, cacheName);
        }

        public QRCodeSettings DefaultSettings
        {
            get
            {
                if (_defaultSettings is null)
                {
                    _defaultSettings = _qrCodeBuilder.GetDefaultSettings(_publishedContent, _propertyAlias);
                }
                return _defaultSettings;
            }
        }
    }
}
