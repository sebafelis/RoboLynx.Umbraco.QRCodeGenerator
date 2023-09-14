using RoboLynx.Umbraco.QRCodeGenerator.Cache;
using RoboLynx.Umbraco.QRCodeGenerator.Models;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes;
using System.IO;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace RoboLynx.Umbraco.QRCodeGenerator
{
    public interface IQRCodeService
    {
        /// <summary>
        /// Get default setting of QR code defined in QR Code Generator property editor.
        /// </summary>
        /// <param name="publishedContent">Published content</param>
        /// <param name="propertyAlias">Property alias in published content</param>
        /// <returns>Stream containing QR code</returns>
        QRCodeSettings? GetDefaultSettings(IPublishedContent publishedContent, string propertyAlias);

        /// <summary>
        /// Get stream containing QR code. Stream can be storing in and getting from specify cache if is defined.
        /// </summary>
        /// <param name="publishedContent">Published content</param>
        /// <param name="propertyAlias">Property alias in published content</param>
        /// <param name="culture">Culture</param>
        /// <param name="settings">QR code settings</param>
        /// <param name="cacheName">Cache name where code is cached</param>
        /// <returns>Stream containing QR code</returns>
        /// <remarks>Property must be of defined with use QR Code Generator property editor.</remarks>
        Stream GetStream(IPublishedContent publishedContent, string propertyAlias, string? culture, QRCodeSettings? settings, string? cacheName = null);

        /// <summary>
        /// Get stream containing QR code. Stream can be storing in and getting from specify cache if is defined.
        /// </summary>
        /// <typeparam name="TCacheRole">Cache role. It's replace cacheName parameter.</typeparam>
        /// <param name="publishedContent">Published content</param>
        /// <param name="propertyAlias">Property alias in published content</param>
        /// <param name="culture">Culture</param>
        /// <param name="settings">QR code settings</param>
        /// <returns>Stream containing QR code</returns>
        /// <remarks>Property must be of defined with use QR Code Generator property editor.</remarks>
        Stream GetStream<TCacheRole>(IPublishedContent publishedContent, string propertyAlias, string? culture, QRCodeSettings? settings) where TCacheRole: class, IQRCodeCacheRole;

        /// <summary>
        /// Get stream containing QR code. Stream can be storing in and getting from specify cache if is defined.
        /// </summary>
        /// <param name="codeType">Type of QR code</param>
        /// <param name="settings">QR code settings</param>
        /// <param name="cacheName">Cache name where QR codes are stored. If <paramref name="cacheName"/> is null then QR code will be always regenerated.</param>
        /// <returns>Stream containing QR code</returns>
        /// <remarks>You can pass any data you wont.</remarks>
        Stream GetStream(IQRCodeType codeType, QRCodeSettings settings, string? cacheName = null);

        /// <summary>
        /// Get stream containing QR code. Stream can be storing in and getting from specify cache if is defined.
        /// </summary>
        /// <typeparam name="TCacheRole">Cache role. It's replace cacheName parameter.</typeparam>
        /// <param name="codeType">Type of QR code</param>
        /// <param name="settings">QR code settings</param>
        /// <param name="cacheName">Cache name where QR codes are stored. If <paramref name="cacheName"/> is null then QR code will be always regenerated.</param>
        /// <returns>Stream containing QR code</returns>
        /// <remarks>You can pass any data you wont.</remarks>
        Stream GetStream<TCacheRole>(IQRCodeType codeType, QRCodeSettings settings) where TCacheRole : class, IQRCodeCacheRole;

        /// <summary>
        /// Purge cache.
        /// </summary>
        /// <typeparam name="TCacheRole">Cache role. It's replace cacheName parameter.</typeparam>
        /// <param name="cacheName">Cache name where codes are cached</param>
        void ClearCache(string cacheName);

        /// <summary>
        /// Purge cache.
        /// </summary>
        /// <typeparam name="TCacheRole">Cache role. It's replace cacheName parameter.</typeparam>
        /// <param name="cacheName">Cache name where codes are cached</param>
        void ClearCache<TCacheRole>() where TCacheRole : class, IQRCodeCacheRole;

        /// <summary>
        /// Remove specific QR code from cache.
        /// </summary>
        /// <param name="cacheName">Cache name where QR codes are stored</param>
        /// <param name="publishedContent">Published content</param>
        /// <param name="propertyAlias">Property alias in published content</param>
        /// <param name="culture">Published Content culture</param>
        /// <param name="settings">QR code settings</param>
        void ClearCache(string cacheName, IPublishedContent publishedContent, string propertyAlias, string? culture, QRCodeSettings? settings = null);

        /// <summary>
        /// Remove specific QR code from cache.
        /// </summary>
        /// <typeparam name="TCacheRole">Cache role. It's replace cacheName parameter.</typeparam>
        /// <param name="publishedContent">Published content</param>
        /// <param name="propertyAlias">Property alias in published content</param>
        /// <param name="culture">Published Content culture</param>
        /// <param name="settings">QR code settings</param>
        void ClearCache<TCacheRole>(IPublishedContent publishedContent, string propertyAlias, string? culture, QRCodeSettings? settings = null) where TCacheRole : class, IQRCodeCacheRole;

        /// <summary>
        /// Remove  specific QR code from cache.
        /// </summary>
        /// <param name="cacheName">Cache name where codes are stored.</param>
        void ClearCache(string cacheName, IQRCodeType codeType, QRCodeSettings settings);

        /// <summary>
        /// Remove  specific QR code from cache.
        /// </summary>
        /// <typeparam name="TCacheRole">Cache role. It's replace cacheName parameter.</typeparam>
        /// <param name="cacheName">Cache name where codes are stored.</param>
        void ClearCache<TCacheRole>(IQRCodeType codeType, QRCodeSettings settings) where TCacheRole : class, IQRCodeCacheRole;
    }
}