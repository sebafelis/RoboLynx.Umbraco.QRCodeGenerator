using RoboLynx.Umbraco.QRCodeGenerator.Cache;
using RoboLynx.Umbraco.QRCodeGenerator.Models;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes;
using System.IO;
using Umbraco.Core.Models.PublishedContent;

namespace RoboLynx.Umbraco.QRCodeGenerator
{
    public interface IQRCodeService
    {
        void ClearCache(string cacheName = null);
        void ClearCache(IPublishedContent publishedContent, string propertyAlias, string culture, QRCodeSettings settings = null, string cacheName = null);
        void ClearCache(IQRCodeType codeType, QRCodeSettings settings, string cacheName = null);
        QRCodeSettings GetDefaultSettings(IPublishedContent publishedContent, string propertyAlias);
        Stream GetStream(IPublishedContent publishedContent, string propertyAlias, string culture, QRCodeSettings settings, string cacheName = null);
        Stream GetStream(IQRCodeType codeType, QRCodeSettings settings, string cacheName = null);
    }
}