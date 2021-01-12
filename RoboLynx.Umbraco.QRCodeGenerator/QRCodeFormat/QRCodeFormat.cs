using RoboLynx.Umbraco.QRCodeGenerator.Controllers;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Umbraco.Core;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat
{
    public abstract class QRCodeFormat : IQRCodeFormat
    {
        public abstract string Id { get; }

        public string Name => ApplicationContext.Current.Services.TextService.Localize($"qrCodeFormats/{GetType().Name.ToFirstLower()}Name");

        public abstract IEnumerable<string> RequiredSettings { get; }

        public virtual string FileName => Guid.NewGuid().ToString();

        public abstract HttpContent ResponseContent(string value, QRCodeSettings settings, UmbracoHelper umbracoHelper);

        protected string ResolveIconUrl(string iconPathOrMediaId, UmbracoHelper umbracoHelper)
        {
            if (int.TryParse(iconPathOrMediaId, out int mediaId))
            {
                return umbracoHelper.TypedMedia(mediaId)?.Url;
            }
            return iconPathOrMediaId;
        }
    }
}
