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
        private readonly Uri _cacheUrl;
        private readonly bool _absoluteModeIsDefault;

        public QRCodeCacheUrlProvider(string absoluteCacheUrl, bool absoluteModeIsDefault)
        {
            if (string.IsNullOrEmpty(absoluteCacheUrl))
            {
                throw new ArgumentException($"'{nameof(absoluteCacheUrl)}' cannot be null or empty.", nameof(absoluteCacheUrl));
            }

            _cacheUrl = new Uri(absoluteCacheUrl);
            _absoluteModeIsDefault = absoluteModeIsDefault;
        }

        public string Url(string path, UrlMode urlMode)
        {
            var absoluteUrl = new Uri(_cacheUrl, path);

            if (urlMode == UrlMode.Relative || (!_absoluteModeIsDefault && (urlMode == UrlMode.Auto || urlMode == UrlMode.Default)))
            {
                return absoluteUrl.PathAndQuery;
            }
            return absoluteUrl.AbsoluteUri;
        }
    }
}
