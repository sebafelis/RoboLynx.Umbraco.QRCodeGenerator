using System;
using System.IO;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace RoboLynx.Umbraco.QRCodeGenerator.Cache
{
    public interface IQRCodeCacheManager
    {
        /// <summary>
        /// Add stream with QR Code to cache.
        /// </summary>
        /// <param name="codeId">Unique code ID base on hash code.</param>
        /// <param name="extension">Extension of file where stream is be save</param>
        /// <param name="stream">Stream containing code</param>
        /// <param name="cacheName">Cache name</param>
        void Add(string hashId, string extension, Stream stream, string cacheName);

        /// <summary>
        /// Clear cache from expired items.
        /// </summary>
        /// <param name="cacheName">Cache name<</param>
        void CleanupCache(string cacheName);

        /// <summary>
        /// Clear cache from specify item.
        /// </summary>
        /// <param name="codeId">Unique code ID base on hash code</param>
        /// <param name="cacheName">Cache name<</param>
        void Clear(string hashId, string cacheName);

        /// <summary>
        /// Clear all cache items.
        /// </summary>
        /// <param name="cacheName">Cache name<</param>
        void ClearAll(string cacheName);

        /// <summary>
        /// Get a stream with specific stored in cache QR code.
        /// </summary>
        /// <param name="codeId">Unique code ID base on hash code</param>
        /// <param name="cacheName">Cache name<</param>
        /// <returns></returns>
        Stream GetStream(string hashId, string cacheName);

        /// <summary>
        /// Get URL address direct to cache file.
        /// </summary>
        /// <param name="hashId">Unique code ID base on hash code</param>
        /// <param name="uriKind">URL mode</param>
        /// <param name="cacheName">Cache name</param>
        /// <returns>URL</returns>
        public string GetUrl(string hashId, UrlMode uriKind, string cacheName);

        /// <summary>
        /// Are direct cache URLs supported.
        /// </summary>
        /// <returns>URL</returns>
        bool UrlSupport(string cacheName);

        /// <summary>
        /// Check is specify QR code in cache.
        /// </summary>
        /// <param name="codeId">Unique code ID base on hash code</param>
        /// <param name="cacheName">Cache name<</param>
        /// <returns></returns>
        bool IsCached(string hashId, string cacheName);

        /// <summary>
        /// Get cache item expiring date/time
        /// </summary>
        /// <param name="hashId">Unique code ID base on hash code</param>
        /// <param name="cacheName">Cache name</param>
        /// <returns>Value or NULL if cache not exist</returns>
        DateTimeOffset? Expired(string hashId, string cacheName);

        /// <summary>
        /// Get cache item last modification date/time
        /// </summary>
        /// <param name="hashId">Unique code ID base on hash code</param>
        /// <param name="cacheName">Cache name</param>
        /// <returns>Value or NULL if cache not exist</returns>
        DateTimeOffset? LastModified(string hashId, string cacheName);

        /// <summary>
        /// Initialize cache. Execute only when server has SchedulingPublisher or Singles state set (<seealso cref="T:Umbraco.Cms.Core.Sync.ServerRoll"/>).
        /// </summary>
        /// <param name="cacheName">Cache name</param>
        void Initialize(string cacheName);
    }
}