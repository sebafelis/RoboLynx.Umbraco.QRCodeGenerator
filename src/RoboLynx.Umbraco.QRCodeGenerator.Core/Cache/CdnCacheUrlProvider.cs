using Microsoft.AspNetCore.Http;
using System;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace RoboLynx.Umbraco.QRCodeGenerator.Cache
{
    public class CdnCacheUrlProvider : LocalCacheUrlProvider
    {
        public CdnCacheUrlProvider(IHttpContextAccessor httpContextAccessor, Uri baseUrl) : base(httpContextAccessor, baseUrl, true)
        {
            if (!baseUrl.IsAbsoluteUri)
            {
                throw new ArgumentException($"{nameof(baseUrl)} must be an absolute URL");
            }
        }

        public override string Url(string path, UrlMode urlMode)
        {
            return base.Url(path, UrlMode.Absolute);
        }
    }
}