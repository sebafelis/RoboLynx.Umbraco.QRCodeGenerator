using RoboLynx.Umbraco.QRCodeGenerator.Models;
using System;
using System.IO;
using Umbraco.Core.Models.PublishedContent;

namespace RoboLynx.Umbraco.QRCodeGenerator.Cache
{
    public interface IQRCodeCache
    {
        /// <summary>
        /// Name of the cache
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Check is the code cached.
        /// </summary>
        /// <param name="codeId">Unique code ID base on hash code.</param>
        /// <returns><c>true</c> if is cached.</returns>
        bool IsCached(string hashId);

        /// <summary>
        /// Get cache item expiring date/time
        /// </summary>
        /// <param name="hashId">Unique code ID base on hash code.</param>
        /// <returns></returns>
        DateTimeOffset? Expired(string hashId);

        /// <summary>
        /// Get stream containing QR code file.
        /// </summary>
        /// <param name="codeId">Unique code ID base on hash code.</param>
        /// <returns>QR code file</returns>
        Stream GetStream(string hashId);

        /// <summary>
        /// Get URL address direct to cache file.
        /// </summary>
        /// <param name="hashId">Unique ID base on hash code.</param>
        /// <param name="uriKind">URL mode</param>
        /// <returns>URL</returns>
        string Url(string hashId, UrlMode urlMode);

        /// <summary>
        /// Are direct cache URLs supported.
        /// </summary>
        /// <returns>URL</returns>
        bool UrlSupport();

        /// <summary>
        /// Add code to the cache.
        /// </summary>
        /// <param name="codeId">Unique code ID base on hash code.</param>
        /// <param name="extension">Extension of file where stream is be save.</param>
        /// <param name="stream">Stream with code.</param>
        void Add(string codeId, string extension, Stream stream);

        /// <summary>
        /// Clear cache from item at specify content and settings.
        /// </summary>
        /// <param name="codeId">Unique code ID base on hash code.</param>
        void Clear(string codeId);

        /// <summary>
        /// Clear cache from all items.
        /// </summary>
        void ClearAll();

        /// <summary>
        /// Cleanup cache from expired items.
        /// </summary>
        void CleanupCache();
    }
}
