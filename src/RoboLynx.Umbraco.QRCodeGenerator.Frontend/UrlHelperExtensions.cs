using RoboLynx.Umbraco.QRCodeGenerator;
using RoboLynx.Umbraco.QRCodeGenerator.Frontend;
using RoboLynx.Umbraco.QRCodeGenerator.Frontend.Controllers;
using RoboLynx.Umbraco.QRCodeGenerator.Models;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes;
using System;
using System.Web.Routing;
using Umbraco.Core.Models.PublishedContent;

namespace Umbraco.Web
{
    public static class UrlHelperExtensions
    {
        #region WebApi URL Helper

        /// <summary>
        /// Gets URL to file containing QR code generated with specify settings.
        /// </summary>
        /// <param name="publishedContent">Content containing property of data type base on <c>qrCodeGenerator</c> property editor.</param>
        /// <param name="propertyAlias">Alias of <c>qrCodeGenerator</c> property.</param>
        /// <param name="settings">QR code settings.</param>
        /// <param name="culture">Documents culture.</param>
        /// <param name="urlMode">Define relative or absolute URL mode.</param>
        /// <returns>URL to file containing QR code<./returns>
        public static string GetQRCodeUrl(this System.Web.Http.Routing.UrlHelper urlHelper, IPublishedContent publishedContent, string propertyAlias, QRCodeSettings settings, string culture, UrlMode urlMode = UrlMode.Auto)
        {
            var routeValues = new RouteValueDictionary(settings)
            {
                { "nodeUdi", publishedContent.GetUdi() },
                { "propertyAlias", propertyAlias },
                { "culture", culture }
            };

            var actionUrl = urlHelper.GetUmbracoApiService<PublicQRCodeController>("Get", routeValues);

            if (urlMode == UrlMode.Absolute)
            {
                var requestUrl = urlHelper.Request.RequestUri;
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
        public static string GetQRCodeUrl(this System.Web.Http.Routing.UrlHelper urlHelper, QRCode qrCodeProperty, QRCodeSettings settings, string culture, UrlMode urlMode = UrlMode.Auto)
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
        public static string GetQRCodeUrl(this System.Web.Http.Routing.UrlHelper urlHelper, QRCode qrCodeProperty, string culture, UrlMode urlMode = UrlMode.Auto)
        {
            return urlHelper.GetQRCodeUrl(qrCodeProperty, qrCodeProperty.DefaultSettings, culture, urlMode);
        }

        /// <summary>
        /// Gets URL to file containing QR code generated with default settings and current culture.
        /// </summary>
        /// <param name="qrCodeProperty">Content property.</param>
        /// <param name="urlMode">Define relative or absolute URL mode.</param>
        /// <returns>URL to file containing QR code<./returns>
        public static string GetQRCodeUrl(this System.Web.Http.Routing.UrlHelper urlHelper, QRCode qrCodeProperty, UrlMode urlMode = UrlMode.Auto)
        {
            return urlHelper.GetQRCodeUrl(qrCodeProperty, qrCodeProperty.DefaultSettings, qrCodeProperty.PublishedContent.GetCultureFromDomains(), urlMode);
        }

        #endregion

        #region MVC URL Helper

        /// <summary>
        /// Gets URL to file containing QR code generated with specify settings.
        /// </summary>
        /// <param name="publishedContent">Content containing property of data type base on <c>qrCodeGenerator</c> property editor.</param>
        /// <param name="propertyAlias">Alias of <c>qrCodeGenerator</c> property.</param>
        /// <param name="settings">QR code settings.</param>
        /// <param name="culture">Documents culture.</param>
        /// <param name="urlMode">Define relative or absolute URL mode.</param>
        /// <returns>URL to file containing QR code<./returns>
        public static string GetQRCodeUrl(this System.Web.Mvc.UrlHelper urlHelper, IPublishedContent publishedContent, string propertyAlias, QRCodeSettings settings, string culture, UrlMode urlMode = UrlMode.Auto)
        {
            var routeValues = new RouteValueDictionary(settings)
            {
                { "nodeUdi", publishedContent.GetUdi() },
                { "propertyAlias", propertyAlias },
                { "culture", culture }
            };

            var actionUrl = urlHelper.GetUmbracoApiService<PublicQRCodeController>("Get", routeValues);

            if (urlMode == UrlMode.Absolute)
            {
                var requestUrl = urlHelper.RequestContext.HttpContext.Request.Url;
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
        public static string GetQRCodeUrl(this System.Web.Mvc.UrlHelper urlHelper, QRCode qrCodeProperty, QRCodeSettings settings, string culture, UrlMode urlMode = UrlMode.Auto)
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
        public static string GetQRCodeUrl(this System.Web.Mvc.UrlHelper urlHelper, QRCode qrCodeProperty, string culture, UrlMode urlMode = UrlMode.Auto)
        {
            return GetQRCodeUrl(urlHelper, qrCodeProperty, qrCodeProperty.DefaultSettings, culture, urlMode);
        }

        /// <summary>
        /// Gets URL to file containing QR code generated with default settings and current culture.
        /// </summary>
        /// <param name="qrCodeProperty">Content property.</param>
        /// <param name="urlMode">Define relative or absolute URL mode.</param>
        /// <returns>URL to file containing QR code<./returns>
        public static string GetQRCodeUrl(this System.Web.Mvc.UrlHelper urlHelper, QRCode qrCodeProperty, UrlMode urlMode = UrlMode.Auto)
        {
            return GetQRCodeUrl(urlHelper, qrCodeProperty, qrCodeProperty.DefaultSettings, qrCodeProperty.PublishedContent.GetCultureFromDomains(), urlMode);
        }

        #endregion
    }
}
