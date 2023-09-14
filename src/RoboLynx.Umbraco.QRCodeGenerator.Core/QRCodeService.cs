using Microsoft.Extensions.DependencyInjection;
using RoboLynx.Umbraco.QRCodeGenerator.Cache;
using RoboLynx.Umbraco.QRCodeGenerator.Models;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes;
using System;
using System.IO;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace RoboLynx.Umbraco.QRCodeGenerator
{
    public class QRCodeService : IQRCodeService
    {
        private readonly IServiceProvider _serviceProvider;

        protected IQRCodeBuilder CodeBuilder { get; }

        public QRCodeService(IQRCodeBuilder codeBuilder, IServiceProvider serviceProvider)
        {
            CodeBuilder = codeBuilder ?? throw new ArgumentNullException(nameof(codeBuilder));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public QRCodeSettings? GetDefaultSettings(IPublishedContent publishedContent, string propertyAlias)
        {
            return CodeBuilder.GetDefaultSettings(publishedContent, propertyAlias);
        }

        public Stream GetStream(IPublishedContent publishedContent, string propertyAlias, string? culture, QRCodeSettings? settings, string? cacheName = null)
        {
            var config = CodeBuilder.CreateConfiguration(publishedContent, propertyAlias, culture, settings);

            return CodeBuilder.CreateStream(config, cacheName);
        }

        public Stream GetStream<TCacheRole>(IPublishedContent publishedContent, string propertyAlias, string? culture, QRCodeSettings? settings) where TCacheRole : class, IQRCodeCacheRole
        {
            return GetStream(publishedContent, propertyAlias, culture, settings, GetCacheNameByRole<TCacheRole>());
        }

        public Stream GetStream(IQRCodeType codeType, QRCodeSettings? settings, string? cacheName = null)
        {
            var config = CodeBuilder.CreateConfiguration(codeType, settings);

            return CodeBuilder.CreateStream(config, cacheName);
        }

        public Stream GetStream<TCacheRole>(IQRCodeType codeType, QRCodeSettings settings) where TCacheRole : class, IQRCodeCacheRole
        {
            return GetStream(codeType, settings, GetCacheNameByRole<TCacheRole>());
        }

        public void ClearCache(string cacheName)
        {
            CodeBuilder.CacheManager.ClearAll(cacheName);
        }

        public void ClearCache<TCacheRole>() where TCacheRole : class, IQRCodeCacheRole
        {
            ClearCache(GetCacheNameByRole<TCacheRole>());
        }

        public void ClearCache(string cacheName, IPublishedContent publishedContent, string propertyAlias, string? culture, QRCodeSettings? settings = null)
        {
            var qrCodeGeneratorConfig = CodeBuilder.CreateConfiguration(publishedContent, propertyAlias, culture, settings);

            var hashId = qrCodeGeneratorConfig.Format.FileName;

            CodeBuilder.CacheManager.Clear(hashId, cacheName);
        }

        public void ClearCache<TCacheRole>(IPublishedContent publishedContent, string propertyAlias, string? culture, QRCodeSettings? settings) where TCacheRole : class, IQRCodeCacheRole
        {
            ClearCache(GetCacheNameByRole<TCacheRole>(), publishedContent, propertyAlias, culture, settings);   
        }

        public void ClearCache(string cacheName, IQRCodeType codeType, QRCodeSettings settings)
        {
            var config = CodeBuilder.CreateConfiguration(codeType, settings);

            var hashId = config.Format.FileName;

            CodeBuilder.CacheManager.Clear(hashId, cacheName);
        }

        public void ClearCache<TCacheRole>(IQRCodeType codeType, QRCodeSettings settings) where TCacheRole : class, IQRCodeCacheRole
        {
            ClearCache(GetCacheNameByRole<TCacheRole>(), codeType, settings);
        }

        private string GetCacheNameByRole<TCacheRole>() where TCacheRole : class, IQRCodeCacheRole
        {
            return _serviceProvider.GetRequiredService<TCacheRole>().Name;
        }
    }
}