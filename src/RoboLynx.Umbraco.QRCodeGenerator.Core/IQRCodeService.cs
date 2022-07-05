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
        /// <param name="culture">Culture</param>
        /// <param name="settings">QR code settings</param>
        /// <param name="cacheName">Cache name where code is cached</param>
        /// <returns>Stream with QR code</returns>
        QRCodeSettings GetDefaultSettings(IPublishedContent publishedContent, string propertyAlias);

        /// <summary>
        /// Get stream with QR code. Stream can be storing in and getting from specify cache if is defined.
        /// </summary>
        /// <param name="publishedContent">Published content</param>
        /// <param name="propertyAlias">Property alias in published content</param>
        /// <param name="culture">Culture</param>
        /// <param name="settings">QR code settings</param>
        /// <param name="cacheName">Cache name where code is cached</param>
        /// <returns>Stream with QR code</returns>
        /// <remarks>Property must be of defined with use QR Code Generator property editor.</remarks>
        Stream GetStream(IPublishedContent publishedContent, string propertyAlias, string? culture, QRCodeSettings? settings, string? cacheName = null);

        /// <summary>
        /// Get stream with QR code. Stream can be storing in and getting from specify cache if is defined.
        /// </summary>
        /// <param name="codeType">Tyoe of QR code</param>
        /// <param name="settings">QR code settings</param>
        /// <param name="cacheName">Cache name where code is cached</param>
        /// <returns>Stream with QR code</returns>
        /// <remarks>You can pass any data you wont.</remarks>
        Stream GetStream(IQRCodeType codeType, QRCodeSettings settings, string? cacheName = null);

        /// <summary>
        /// Clear all specify cache.
        /// </summary>
        /// <param name="cacheName">Cache name where codes are cached</param>
        void ClearCache(string? cacheName = null);

        /// <summary>
        /// Remove from specific cache a file of specific QR code.
        /// </summary>
        /// <param name="cacheName">Cache name where codes are cached</param>
        void ClearCache(IPublishedContent publishedContent, string propertyAlias, string? culture, QRCodeSettings? settings = null, string? cacheName = null);

        /// <summary>
        /// Remove from specific cache a file of specific QR code.
        /// </summary>
        /// <param name="cacheName">Cache name where codes are cached</param>
        void ClearCache(IQRCodeType codeType, QRCodeSettings settings, string? cacheName = null);
    }
}