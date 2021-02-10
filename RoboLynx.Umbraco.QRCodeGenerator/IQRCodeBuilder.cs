using RoboLynx.Umbraco.QRCodeGenerator.Controllers;
using RoboLynx.Umbraco.QRCodeGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models.PublishedContent;

namespace RoboLynx.Umbraco.QRCodeGenerator.Extensions
{
    public interface IQRCodeBuilder
    {
        QRCodeSettings GetDefaultSettings(IPublishedContent publishedContent, string propertyAlias);

        QRCodeConfig CreateConfiguration(IPublishedContent publishedContent, string propertyAlias, QRCodeSettings userSettings);

        HttpContent CreateQRCodeAsResponse(IPublishedContent publishedContent, string propertyAlias, QRCodeSettings userSettings);
    }
}