using Microsoft.AspNetCore.Mvc;
using RoboLynx.Umbraco.QRCodeGenerator.Exceptions;
using RoboLynx.Umbraco.QRCodeGenerator.Models;
using System;
using System.Linq;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace RoboLynx.Umbraco.QRCodeGenerator
{
    public class QRCodeResponesFactory : IQRCodeResponesFactory
    {
        private readonly IQRCodeBuilder _qrCodeBuilder;

        public QRCodeResponesFactory(IQRCodeBuilder codeBuilder)
        {
            _qrCodeBuilder = codeBuilder;
        }

        public IActionResult CreateResponesWithQRCode(IPublishedContent? publishedContent, string? propertyAlias, string? culture, QRCodeSettings? settings, string? cacheName)
        {
            if (publishedContent != null && !string.IsNullOrEmpty(propertyAlias))
            {
                try
                {
                    var config = _qrCodeBuilder.CreateConfiguration(publishedContent, propertyAlias, culture, settings);

                    if (config is null)
                    {
                        return new BadRequestResult();
                    }

                    var hashId = config.Format.HashId;

                    if (!_qrCodeBuilder.CacheManager.UrlSupport(cacheName))
                    {
                        var stream = _qrCodeBuilder.CreateStream(config, cacheName);

                        var lastModified = _qrCodeBuilder.CacheManager.LastModified(hashId, cacheName);

                        var respones = new FileStreamResult(stream, config.Format.Mime);
                        if (lastModified.HasValue)
                        {
                            respones.LastModified = lastModified;
                            respones.EntityTag = new Microsoft.Net.Http.Headers.EntityTagHeaderValue($"\"${TurnDatetimeOffsetToETag(lastModified.Value)}\"");
                        }
                        return respones;
                    }
                    else
                    {
                        if (!_qrCodeBuilder.CacheManager.IsCached(hashId, cacheName))
                        {
                            _ = _qrCodeBuilder.CreateStream(config, cacheName);
                        }
                        var cacheUrl = _qrCodeBuilder.CacheManager.GetUrl(hashId, UrlMode.Default, cacheName);

                        if (cacheUrl == null)
                        {
                            throw new NullReferenceException("URL can not be generate.");
                        }
                        return new RedirectResult(cacheUrl, false);
                    }
                }
                catch (QRCodeGeneratorException qrex)
                {
                    return new BadRequestObjectResult(qrex.Message);
                }
                catch (Exception ex) when (ex is ArgumentException || ex is ArgumentNullException)
                {
                    return new BadRequestResult();
                }
            }

            return new NotFoundResult();
        }

        public IActionResult CreateResponseWithDefaultSettings(IPublishedContent? publishedContent, string? propertyAlias)
        {
            if (publishedContent != null && !string.IsNullOrEmpty(propertyAlias))
            {
                try
                {
                    var defaultSettings = _qrCodeBuilder.GetDefaultSettings(publishedContent, propertyAlias);

                    if (defaultSettings is null)
                    {
                        return new BadRequestObjectResult("Content has not configuration.");
                    }

                    return new OkObjectResult(defaultSettings);
                }
                catch (QRCodeGeneratorException qrex)
                {
                    return new BadRequestObjectResult(qrex.Message);
                }
                catch (Exception ex) when (ex is ArgumentException || ex is ArgumentNullException)
                {
                    return new BadRequestResult();
                }
            }

            return new NotFoundResult();
        }

        private static string TurnDatetimeOffsetToETag(DateTimeOffset dateTimeOffset)
        {
            var dateBytes = BitConverter.GetBytes(dateTimeOffset.UtcDateTime.Ticks);
            var offsetBytes = BitConverter.GetBytes((Int16)dateTimeOffset.Offset.TotalHours);
            return Convert.ToBase64String(dateBytes.Concat(offsetBytes).ToArray());
        }
    }
}