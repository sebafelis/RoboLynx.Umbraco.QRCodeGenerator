using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Umbraco.Core;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;

namespace RoboLynx.Umbraco.QRCodeGenerator.Cache
{
    public class LocalCacheUrlProvider : IQRCodeCacheUrlProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Uri _baseUrl;
        private readonly bool _absoluteModeIsDefault;

        public LocalCacheUrlProvider(IHttpContextAccessor httpContextAccessor, Uri baseUrl, bool absoluteModeIsDefault)
        {
            if (baseUrl is null)
            {
                throw new ArgumentNullException($"'{nameof(baseUrl)}' cannot be null or empty.", nameof(baseUrl));
            }
            _httpContextAccessor = httpContextAccessor;
            _baseUrl = baseUrl;
            _absoluteModeIsDefault = absoluteModeIsDefault;
        }

        public string Url(string path, UrlMode urlMode)
        {
            Uri url = new Uri(_baseUrl.ToString().EnsureEndsWith("/") + path.TrimStart("/"), UriKind.RelativeOrAbsolute);
            
            if (!url.IsAbsoluteUri)
            {
                var requestUrl = _httpContextAccessor.HttpContext.Request.Url;

                if (requestUrl is not null && requestUrl.IsAbsoluteUri)
                {
                    url = new Uri(requestUrl, url);
                }
                else
                {
                    throw new InvalidOperationException("Could not build valid absolute URL.");
                }
            }

            if (urlMode == UrlMode.Relative || (!_absoluteModeIsDefault && (urlMode == UrlMode.Auto || urlMode == UrlMode.Default)))
            {
                return url.PathAndQuery;
            }

            return url.AbsoluteUri;
        }
    }
}
