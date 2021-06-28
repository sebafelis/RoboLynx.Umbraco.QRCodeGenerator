using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models.PublishedContent;

namespace RoboLynx.Umbraco.QRCodeGenerator.Cache
{
    public class QRCodeCacheUrlProvider : IQRCodeCacheUrlProvider
    {
        private readonly Uri _cacheLocation;
        private readonly bool _defaultAbsolute;

        public QRCodeCacheUrlProvider(string cacheLocationUrl, bool defaultAbsolute)
        {
            _cacheLocation = new Uri(cacheLocationUrl);
            _defaultAbsolute = defaultAbsolute;
        }

        public string Url(string path, UrlMode urlMode)
        {
            var absoluteUrl = new Uri(_cacheLocation, path);

            if (urlMode == UrlMode.Relative || (!_defaultAbsolute && (urlMode == UrlMode.Auto || urlMode == UrlMode.Default)))
            {
                return absoluteUrl.PathAndQuery;
            }
            return absoluteUrl.AbsoluteUri;
        }
    }
}
