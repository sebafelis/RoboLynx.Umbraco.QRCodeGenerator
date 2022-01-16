using Microsoft.AspNetCore.Routing;
using RoboLynx.Umbraco.QRCodeGenerator;
using RoboLynx.Umbraco.QRCodeGenerator.Frontend;
using RoboLynx.Umbraco.QRCodeGenerator.Frontend.Controllers;
using RoboLynx.Umbraco.QRCodeGenerator.Models;
using System;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;
using Microsoft.AspNetCore.Http.Extensions;
using Umbraco.Cms.Core;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Umbraco.Cms.Web.Common.Controllers;

namespace Umbraco.Extensions
{
    public static class UrlHelperExtensions
    {
        /// <summary>
        /// Gets URL to file containing QR code generated with specify settings.
        /// </summary>
        /// <param name="publishedContent">Content containing property of data type base on <c>qrCodeGenerator</c> property editor.</param>
        /// <param name="propertyAlias">Alias of <c>qrCodeGenerator</c> property.</param>
        /// <param name="settings">QR code settings.</param>
        /// <param name="culture">Documents culture.</param>
        /// <param name="urlMode">Define relative or absolute URL mode.</param>
        /// <returns>URL to file containing QR code<./returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0037:Use inferred member name", Justification = "<Pending>")]
        public static string GetQRCodeUrl(this IUrlHelper urlHelper, IPublishedContent publishedContent, string propertyAlias, QRCodeSettings settings, string culture, UrlMode urlMode = UrlMode.Auto)
        {
            var controllerType = typeof(PublicQRCodeController);

            var metaData = PluginController.GetMetadata(controllerType);

            var routeValues = new
            {
                darkColor = settings.DarkColor,
                drawQuiteZone = settings.DrawQuiteZone,
                eCCLevel = settings.ECCLevel,
                format = settings.Format,
                icon = settings.Icon,
                iconBorderWidth = settings.IconBorderWidth,
                iconSizePercent = settings.IconSizePercent,
                lightColor = settings.LightColor,
                size = settings.Size,
                nodeUdi = publishedContent.GetUdi(),
                propertyAlias = propertyAlias,
                culture = culture,
                area = metaData.AreaName
            };

            var actionUrl = urlHelper.Action("Get", metaData.ControllerName, routeValues);

            if (urlMode == UrlMode.Absolute)
            {
                var requestUrl = new Uri(urlHelper.ActionContext.HttpContext.Request.GetEncodedUrl());
                var rootUrl = $"{requestUrl.Scheme}://{requestUrl.Host}";
                if (!requestUrl.IsDefaultPort)
                    rootUrl += $":{requestUrl.Port}";

                return new Uri(new Uri(rootUrl), actionUrl).AbsoluteUri;
            }
            return actionUrl;
        }
         
        /// <summary>
        /// Gets URL to file containing QR code generated with specify settings.
        /// </summary>
        /// <param name="qrCodeProperty">Content property.</param>
        /// <param name="settings">QR code settings.</param>
        /// <param name="culture">Documents culture.</param>
        /// <param name="urlMode">Define relative or absolute URL mode.</param>
        /// <returns>URL to file containing QR code<./returns>
        public static string GetQRCodeUrl(this IUrlHelper urlHelper, QRCode qrCodeProperty, QRCodeSettings settings, string culture, UrlMode urlMode = UrlMode.Auto)
        {
            return urlHelper.GetQRCodeUrl(qrCodeProperty.PublishedContent, qrCodeProperty.PropertyAlias, settings, culture, urlMode);
        }

        /// <summary>
        /// Gets URL to file containing QR code generated with default settings.
        /// </summary>
        /// <param name="qrCodeProperty">Content property.</param>
        /// <param name="culture">Documents culture.</param>
        /// <param name="urlMode">Define relative or absolute URL mode.</param>
        /// <returns>URL to file containing QR code<./returns>
        public static string GetQRCodeUrl(this IUrlHelper urlHelper, QRCode qrCodeProperty, string culture, UrlMode urlMode = UrlMode.Auto)
        {
            return urlHelper.GetQRCodeUrl(qrCodeProperty, qrCodeProperty.DefaultSettings, culture, urlMode);
        }

        /// <summary>
        /// Gets URL to file containing QR code generated with default settings and current culture.
        /// </summary>
        /// <param name="qrCodeProperty">Content property.</param>
        /// <param name="urlMode">Define relative or absolute URL mode.</param>
        /// <returns>URL to file containing QR code<./returns>
        public static string GetQRCodeUrl(this IUrlHelper urlHelper, QRCode qrCodeProperty, UrlMode urlMode = UrlMode.Auto)
        {
            return urlHelper.GetQRCodeUrl(qrCodeProperty, qrCodeProperty.DefaultSettings, qrCodeProperty.PublishedContent.GetCultureFromDomains(), urlMode);
        }
    }
}
