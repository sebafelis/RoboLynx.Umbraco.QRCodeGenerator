using RoboLynx.Umbraco.QRCodeGenerator.Controllers;
using RoboLynx.Umbraco.QRCodeGenerator.Models;
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
        protected readonly ILocalizedTextService localizedTextService;
        protected readonly UmbracoHelper umbracoHelper;

        public QRCodeFormat(ILocalizedTextService localizedTextService, UmbracoHelper umbracoHelper)
        {
            this.localizedTextService = localizedTextService;
            this.umbracoHelper = umbracoHelper;
        }

        public abstract string Id { get; }

        public string Name => localizedTextService.Localize($"qrCodeFormats/{GetType().Name.ToFirstLower()}Name");

        public abstract string Mime { get; }

        public abstract IEnumerable<string> RequiredSettings { get; }

        public virtual string FileName => Guid.NewGuid().ToString();

        public abstract HttpContent ResponseContent(string value, QRCodeSettings settings);

        protected string ResolveIconUrl(string iconPathOrMediaId)
        {
            if (!string.IsNullOrEmpty(iconPathOrMediaId))
            {
                if (int.TryParse(iconPathOrMediaId, out int mediaId))
                {
                    return umbracoHelper.Media(mediaId)?.Url();
                }
                return iconPathOrMediaId;
            }
            return null;
        }
    }
}
