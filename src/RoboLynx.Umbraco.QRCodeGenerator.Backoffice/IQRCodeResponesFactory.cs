using Microsoft.AspNetCore.Mvc;
using RoboLynx.Umbraco.QRCodeGenerator.Models;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace RoboLynx.Umbraco.QRCodeGenerator
{
    public interface IQRCodeResponesFactory
    {
        IActionResult CreateResponesWithQRCode(IPublishedContent? publishedContent, string? propertyAlias, string? culture, QRCodeSettings? settings, string? cacheName);

        IActionResult CreateResponseWithDefaultSettings(IPublishedContent? publishedContent, string? propertyAlias);
    }
}