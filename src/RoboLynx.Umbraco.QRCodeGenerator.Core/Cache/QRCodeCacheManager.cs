using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Umbraco.Core.Models.PublishedContent;

namespace RoboLynx.Umbraco.QRCodeGenerator.Cache
{
    public class QRCodeCacheManager : IQRCodeCacheManager
    {
        private readonly IDictionary<string, IQRCodeCache> _caches;

        public QRCodeCacheManager(IEnumerable<IQRCodeCache> caches)
        {
            _caches = caches?.ToDictionary(k=>k.Name, v=>v) ?? new Dictionary<string, IQRCodeCache>();
        }

        private IQRCodeCache GetCache(string cacheName)
        {
            if (_caches.ContainsKey(cacheName))
            {
                return _caches[cacheName];
            }
            return null;
        }

        public bool IsCached(string hashId, string cacheName)
        {
            return GetCache(cacheName)?.IsCached(hashId) ?? false;
        }

        public Stream GetStream(string hashId, string cacheName)
        {
            return GetCache(cacheName)?.GetStream(hashId);
        }

        public void Add(string hashId, string extension, Stream stream, string cacheName)
        {
            GetCache(cacheName)?.Add(hashId, extension, stream);
        }

        public void Clear(string hashId, string cacheName)
        {
            GetCache(cacheName)?.Clear(hashId);
        }

        public void CleanupCache(string cacheName)
        {
            GetCache(cacheName)?.CleanupCache();
        }

        public void ClearAll(string cacheName)
        {
            GetCache(cacheName)?.ClearAll();
        }

        public string GetUrl(string hashId, UrlMode uriKind, string cacheName)
        {
            if (!UrlSupport(cacheName))
            {
                throw new NotSupportedException();
            }
            return GetCache(cacheName)?.Url(hashId, uriKind);
        }

        public bool UrlSupport(string cacheName)
        {
            return GetCache(cacheName)?.UrlSupport() ?? false;
        }

        public DateTimeOffset? Expired(string hashId, string cacheName)
        {
            return GetCache(cacheName)?.Expired(hashId);
        }
    }
}
