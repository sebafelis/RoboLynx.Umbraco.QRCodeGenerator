using RoboLynx.Umbraco.QRCodeGenerator.Cache;
using RoboLynx.Umbraco.QRCodeGenerator.Models;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes;
using System;
using System.IO;
using System.Net.Http;
using System.Web;
using Umbraco.Core.Models.PublishedContent;

namespace RoboLynx.Umbraco.QRCodeGenerator
{
    public interface IQRCodeBuilder
    {
        public IQRCodeCacheManager CacheManager { get; }
        QRCodeConfig CreateConfiguration(IPublishedContent publishedContent, string propertyAlias, string culture, QRCodeSettings userSettings);
        QRCodeConfig CreateConfiguration(IQRCodeType codeType, QRCodeSettings userSettings);
        HttpResponseMessage CreateResponse(HttpRequestMessage request, QRCodeConfig config, bool attachment = false, string cacheName = null);
        Stream CreateStream(QRCodeConfig config, string cacheName);
        string GetUrl(QRCodeConfig config, string cacheName);
        QRCodeSettings GetDefaultSettings(IPublishedContent publishedContent, string propertyAlias);
        IQRCodeFormat GetFormat(IQRCodeType codeType, QRCodeSettings settings);
    }
}