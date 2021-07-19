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
        protected IQRCodeBuilder CodeBuilder { get; }

        public QRCodeService(IQRCodeBuilder codeBuilder)
        {
            CodeBuilder = codeBuilder ?? throw new System.ArgumentNullException(nameof(codeBuilder));
        }

        public Stream GetStream(IPublishedContent publishedContent, string propertyAlias, string culture, QRCodeSettings settings, string cacheName = null)
        {
            var config = CodeBuilder.CreateConfiguration(publishedContent, propertyAlias, culture, settings);

            return CodeBuilder.CreateStream(config, cacheName);
        }

        public Stream GetStream(IQRCodeType codeType, QRCodeSettings settings, string cacheName = null)
        {
            var config = CodeBuilder.CreateConfiguration(codeType, settings);

            return CodeBuilder.CreateStream(config, cacheName);
        }

        public QRCodeSettings GetDefaultSettings(IPublishedContent publishedContent, string propertyAlias)
        {
            return CodeBuilder.GetDefaultSettings(publishedContent, propertyAlias);
        }

        public void ClearCache(IPublishedContent publishedContent, string propertyAlias, string culture, QRCodeSettings settings = null, string cacheName = null)
        {
            var qrCodeGeneratorConfig = CodeBuilder.CreateConfiguration(publishedContent, propertyAlias, culture, settings);

            var hashId = qrCodeGeneratorConfig.Format.FileName;

            CodeBuilder.CacheManager.Clear(hashId, cacheName);
        }

        public void ClearCache(IQRCodeType codeType, QRCodeSettings settings, string cacheName = null)
        {
            var config = CodeBuilder.CreateConfiguration(codeType, settings);

            var hashId = config.Format.FileName;

            CodeBuilder.CacheManager.Clear(hashId, cacheName);
        }

        public void ClearCache(string cacheName = null)
        {
            CodeBuilder.CacheManager.ClearAll(cacheName);
        }
    }
}
