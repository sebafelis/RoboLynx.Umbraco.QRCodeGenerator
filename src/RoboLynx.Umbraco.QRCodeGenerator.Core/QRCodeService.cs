using RoboLynx.Umbraco.QRCodeGenerator.Cache;
using RoboLynx.Umbraco.QRCodeGenerator.Models;
using System.IO;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.Services;
using Umbraco.Web;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes;
using System.Net.Http;

namespace RoboLynx.Umbraco.QRCodeGenerator
{
    public class QRCodeService : IQRCodeService
    {
        private readonly IQRCodeBuilder _codeBuilder;

        public IQRCodeCacheManager CacheManager { get; }

        internal QRCodeService(IQRCodeBuilder codeBuilder)
        {
            _codeBuilder = codeBuilder;
        }

        public Stream GetStream(IPublishedContent publishedContent, string propertyAlias, string culture, QRCodeSettings settings, string cacheName = null)
        {
            var config = _codeBuilder.CreateConfiguration(publishedContent, propertyAlias, culture, settings);

            return _codeBuilder.CreateQRCodeAsStream(config, cacheName);
        }

        public Stream GetStream(IQRCodeType codeType, QRCodeSettings settings, string cacheName = null)
        {
            var config = _codeBuilder.CreateConfiguration(codeType, settings);

            return _codeBuilder.CreateQRCodeAsStream(config, cacheName);
        }

        public QRCodeSettings GetDefaultSettings(IPublishedContent publishedContent, string propertyAlias)
        {
            return _codeBuilder.GetDefaultSettings(publishedContent, propertyAlias);
        }

        public void ClearCache(IPublishedContent publishedContent, string propertyAlias, string culture, QRCodeSettings settings = null, string cacheName = null)
        {
            var qrCodeGeneratorConfig = _codeBuilder.CreateConfiguration(publishedContent, propertyAlias, culture, settings);

            var hashId = qrCodeGeneratorConfig.Format.FileName;

            CacheManager.Clear(hashId, cacheName);
        }

        public void ClearCache(IQRCodeType codeType, QRCodeSettings settings, string cacheName = null)
        {
            var config = _codeBuilder.CreateConfiguration(codeType, settings);

            var hashId = config.Format.FileName;

            CacheManager.Clear(hashId, cacheName);
        }

        public void ClearCache(string cacheName = null)
        {
            CacheManager.ClearAll(cacheName);
        }
    }
}
