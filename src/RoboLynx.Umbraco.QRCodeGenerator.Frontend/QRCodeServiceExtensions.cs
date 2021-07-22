using RoboLynx.Umbraco.QRCodeGenerator.Models;
using System.IO;
using RoboLynx.Umbraco.QRCodeGenerator.Frontend;

namespace RoboLynx.Umbraco.QRCodeGenerator
{
    public static class QRCodeServiceExtensions
    {
        public static Stream GetStream(this IQRCodeService service, QRCode property, string culture, QRCodeSettings settings)
        {
            return service.GetStream(property.PublishedContent, property.PropertyAlias, culture, settings, property.CacheName);
        }

        public static Stream GetStream(this IQRCodeService service, QRCode property, string culture, QRCodeSettings settings, string cacheName)
        {
            return service.GetStream(property.PublishedContent, property.PropertyAlias, culture, settings, cacheName);
        }

        public static QRCodeSettings GetDefaultSettings(this IQRCodeService service, QRCode property)
        {
            return service.GetDefaultSettings(property.PublishedContent, property.PropertyAlias);
        }
    }
}
