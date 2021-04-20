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

        public virtual string Name => localizedTextService.Localize($"qrCodeFormats/{GetType().Name.ToFirstLower()}Name");

        public abstract string Mime { get; }

        public abstract IEnumerable<string> RequiredSettings { get; }

        public virtual string FileName => Guid.NewGuid().ToString();

        public abstract HttpContent ResponseContent(string value, QRCodeSettings settings);

        /// <summary>
        /// Resolve icon source
        /// </summary>
        /// <param name="icon">Image path, Media ID or Media UDI</param>
        /// <returns>Image path</returns>
        protected string ResolveIconUrl(string icon)
        {
            if (!string.IsNullOrEmpty(icon))
            {
                if (int.TryParse(icon, out int mediaId))
                {
                    return umbracoHelper.Media(mediaId)?.Url();
                }
                if (Udi.TryParse(icon, out Udi mediaUdi))
                {
                    return umbracoHelper.Media(mediaUdi)?.Url();
                }
                return icon;
            }
            return null;
        }
    }
}
