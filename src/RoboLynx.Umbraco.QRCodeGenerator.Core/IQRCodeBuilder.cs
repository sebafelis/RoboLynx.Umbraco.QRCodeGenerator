using RoboLynx.Umbraco.QRCodeGenerator.Models;
using System.Net.Http;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;

namespace RoboLynx.Umbraco.QRCodeGenerator.Extensions
{
    public interface IQRCodeBuilder
    {
        QRCodeSettings GetDefaultSettings(IPublishedContent publishedContent, string propertyAlias);

        QRCodeConfig CreateConfiguration(IPublishedContent publishedContent, string propertyAlias, QRCodeSettings userSettings);

        HttpContent CreateQRCodeAsResponse(IPublishedContent publishedContent, string propertyAlias, string culture, QRCodeSettings userSettings);
    }
}