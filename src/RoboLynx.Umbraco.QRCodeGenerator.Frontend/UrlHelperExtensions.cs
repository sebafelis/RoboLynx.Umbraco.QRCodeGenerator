using RoboLynx.Umbraco.QRCodeGenerator.Frontend;
using RoboLynx.Umbraco.QRCodeGenerator.Frontend.Controllers;
using RoboLynx.Umbraco.QRCodeGenerator.Models;
using System;
using System.Web.Routing;
using Umbraco.Core.Models.PublishedContent;

namespace Umbraco.Web
{
    public static class UrlHelperExtensions
    {
        public static string GetQRCodeUrl(this System.Web.Http.Routing.UrlHelper urlHelper, QRCode qrCodeProperty, QRCodeSettings settings, string culture, UrlMode urlMode = UrlMode.Auto)
        {
            var routeValues = new RouteValueDictionary(settings)
            {
                { "nodeId", qrCodeProperty.PublishedContent.Id },
                { "propertyAlias", qrCodeProperty.PropertyAlias },
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

        public static string GetQRCodeUrl(this System.Web.Http.Routing.UrlHelper urlHelper, QRCode qrCodeProperty, string culture, UrlMode urlMode = UrlMode.Auto)
        {
            return GetQRCodeUrl(urlHelper, qrCodeProperty, qrCodeProperty.DefaultSettings, culture, urlMode);
        }

        public static string GetQRCodeUrl(this System.Web.Http.Routing.UrlHelper urlHelper, QRCode qrCodeProperty, UrlMode urlMode = UrlMode.Auto)
        {
            return GetQRCodeUrl(urlHelper, qrCodeProperty, qrCodeProperty.DefaultSettings, qrCodeProperty.PublishedContent.GetCultureFromDomains(), urlMode);
        }

        public static string GetQRCodeUrl(this System.Web.Mvc.UrlHelper urlHelper, QRCode qrCodeProperty, QRCodeSettings settings, string culture, UrlMode urlMode = UrlMode.Auto)
        {
            var routeValues = new RouteValueDictionary(settings)
            {
                { "nodeId", qrCodeProperty.PublishedContent.Id },
                { "propertyAlias", qrCodeProperty.PropertyAlias },
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

        public static string GetQRCodeUrl(this System.Web.Mvc.UrlHelper urlHelper, QRCode qrCodeProperty, string culture, UrlMode urlMode = UrlMode.Auto)
        {
            return GetQRCodeUrl(urlHelper, qrCodeProperty, qrCodeProperty.DefaultSettings, culture, urlMode);
        }

        public static string GetQRCodeUrl(this System.Web.Mvc.UrlHelper urlHelper, QRCode qrCodeProperty, UrlMode urlMode = UrlMode.Auto)
        {
            return GetQRCodeUrl(urlHelper, qrCodeProperty, qrCodeProperty.DefaultSettings, qrCodeProperty.PublishedContent.GetCultureFromDomains(), urlMode);
        }
    }
}
