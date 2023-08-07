using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using RoboLynx.Umbraco.QRCodeGenerator.Extensions;
using RoboLynx.Umbraco.QRCodeGenerator.Frontend.Controllers;
using RoboLynx.Umbraco.QRCodeGenerator.Models;
using System;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Web.Common.Controllers;
using Umbraco.Extensions;

namespace RoboLynx.Umbraco.QRCodeGenerator.Frontend
{
    public static class UrlHelperExtensionsForQRCodeGenerator
    {
        internal static IQueryCipher? QueryCipher { get; set; }

        private static string? CreateQRCodeUrl(this IUrlHelper urlHelper, IPublishedContent publishedContent, string propertyAlias, QRCodeSettings? settings, string? culture, UrlMode urlMode = UrlMode.Auto)
        {
            var controllerType = typeof(PublicQRCodeController);

            var metaData = PluginController.GetMetadata(controllerType);

            var routeValues = settings != null ? new
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
                propertyAlias,
                culture,
                area = metaData.AreaName
            } : null;

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

        #region GetQRCodeUrl methods 

        /// <summary>
        /// Gets URL to file containing QR code generated with specify settings.
        /// </summary>
        /// <param name="publishedContent">Content containing property of data type base on <c>qrCodeGenerator</c> property editor.</param>
        /// <param name="propertyAlias">Alias of <c>qrCodeGenerator</c> property.</param>
        /// <param name="settings">QR code settings.</param>
        /// <param name="culture">Documents culture.</param>
        /// <param name="urlMode">Define relative or absolute URL mode.</param>
        /// <returns>URL to file containing QR code<./returns>
        public static string? GetQRCodeUrl(this IUrlHelper urlHelper, IPublishedContent publishedContent, string propertyAlias, QRCodeSettings? settings, string? culture, UrlMode urlMode = UrlMode.Auto)
        {
            if (QueryCipher != null && QueryCipher.OnlyCriptedCalls)
            {
                return urlHelper.GetSecureQRCodeUrl(publishedContent, propertyAlias, settings, culture, urlMode);
            }

            return urlHelper.CreateQRCodeUrl(publishedContent, propertyAlias, settings, culture, urlMode);
        }

        /// <summary>
        /// Gets URL to file containing QR code generated with specify settings.
        /// </summary>
        /// <param name="qrCodeProperty">Content property.</param>
        /// <param name="settings">QR code settings.</param>
        /// <param name="culture">Documents culture.</param>
        /// <param name="urlMode">Define relative or absolute URL mode.</param>
        /// <returns>URL to file containing QR code<./returns>
        public static string? GetQRCodeUrl(this IUrlHelper urlHelper, QRCode qrCodeProperty, QRCodeSettings? settings, string? culture, UrlMode urlMode = UrlMode.Auto)
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
        public static string? GetQRCodeUrl(this IUrlHelper urlHelper, QRCode qrCodeProperty, string? culture, UrlMode urlMode = UrlMode.Auto)
        {
            return urlHelper.GetQRCodeUrl(qrCodeProperty, qrCodeProperty.DefaultSettings, culture, urlMode);
        }

        /// <summary>
        /// Gets URL to file containing QR code generated with default settings and current culture.
        /// </summary>
        /// <param name="qrCodeProperty">Content property.</param>
        /// <param name="urlMode">Define relative or absolute URL mode.</param>
        /// <returns>URL to file containing QR code<./returns>
        public static string? GetQRCodeUrl(this IUrlHelper urlHelper, QRCode qrCodeProperty, UrlMode urlMode = UrlMode.Auto)
        {
            return urlHelper.GetQRCodeUrl(qrCodeProperty, qrCodeProperty.DefaultSettings, qrCodeProperty.PublishedContent.GetCultureFromDomains(), urlMode);
        }

        #endregion

        #region GetSecureQRCodeUrl methods

        /// <summary>
        /// Gets secure URL to file containing QR code generated with specify settings. Setting are in encrypted form.
        /// </summary>
        /// <param name="publishedContent">Content containing property of data type base on <c>qrCodeGenerator</c> property editor.</param>
        /// <param name="propertyAlias">Alias of <c>qrCodeGenerator</c> property.</param>
        /// <param name="settings">QR code settings.</param>
        /// <param name="culture">Documents culture.</param>
        /// <param name="urlMode">Define relative or absolute URL mode.</param>
        /// <returns>URL to file containing QR code<./returns>
        public static string? GetSecureQRCodeUrl(this IUrlHelper urlHelper, IPublishedContent publishedContent, string propertyAlias, QRCodeSettings? settings, string? culture, UrlMode urlMode = UrlMode.Auto)
        {
            if (QueryCipher == null)
            {
                throw new ArgumentNullException(nameof(QueryCipher), "QueryCipher is not define.");
            }

            var urlString = urlHelper.CreateQRCodeUrl(publishedContent, propertyAlias, settings, culture, urlMode);

            if (urlString == null) return null;

            var query = urlString[urlString.IndexOf("?")..];

            var encryptedQuery = QueryCipher.EncryptQuery(query);

            var encryptedUrl = $"{urlString[..urlString.IndexOf("?")]}?{Frontend.CryptoParamName}={encryptedQuery}";

            return encryptedUrl;
        }

        /// <summary>
        /// Gets secure URL to file containing QR code generated with specify settings. Setting are in encrypted form.
        /// </summary>
        /// <param name="qrCodeProperty">Content property.</param>
        /// <param name="settings">QR code settings.</param>
        /// <param name="culture">Documents culture.</param>
        /// <param name="urlMode">Define relative or absolute URL mode.</param>
        /// <returns>URL to file containing QR code<./returns>
        public static string? GetSecureQRCodeUrl(this IUrlHelper urlHelper, QRCode qrCodeProperty, QRCodeSettings? settings, string? culture, UrlMode urlMode = UrlMode.Auto)
        {
            return urlHelper.GetSecureQRCodeUrl(qrCodeProperty.PublishedContent, qrCodeProperty.PropertyAlias, settings, culture, urlMode);
        }

        /// <summary>
        /// Gets secure URL to file containing QR code generated with default settings. Setting are in encrypted form.
        /// </summary>
        /// <param name="qrCodeProperty">Content property.</param>
        /// <param name="culture">Documents culture.</param>
        /// <param name="urlMode">Define relative or absolute URL mode.</param>
        /// <returns>URL to file containing QR code<./returns>
        public static string? GetSecureQRCodeUrl(this IUrlHelper urlHelper, QRCode qrCodeProperty, string? culture, UrlMode urlMode = UrlMode.Auto)
        {
            return urlHelper.GetSecureQRCodeUrl(qrCodeProperty, qrCodeProperty.DefaultSettings, culture, urlMode);
        }

        /// <summary>
        /// Gets secure URL to file containing QR code generated with default settings and current culture. Setting are in encrypted form.
        /// </summary>
        /// <param name="qrCodeProperty">Content property.</param>
        /// <param name="urlMode">Define relative or absolute URL mode.</param>
        /// <returns>URL to file containing QR code<./returns>
        public static string? GetSecureQRCodeUrl(this IUrlHelper urlHelper, QRCode qrCodeProperty, UrlMode urlMode = UrlMode.Auto)
        {
            return urlHelper.GetSecureQRCodeUrl(qrCodeProperty, qrCodeProperty.DefaultSettings, qrCodeProperty.PublishedContent.GetCultureFromDomains(), urlMode);
        }

        #endregion
    }
}