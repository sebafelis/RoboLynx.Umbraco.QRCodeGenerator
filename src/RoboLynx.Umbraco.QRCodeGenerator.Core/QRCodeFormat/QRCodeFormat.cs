using Microsoft.Extensions.Logging;
using RoboLynx.Umbraco.QRCodeGenerator.Models;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes;
using System.IO;
using Umbraco.Cms.Core;
using Umbraco.Cms.Web.Common;
using Umbraco.Extensions;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat
{
    public abstract class QRCodeFormat : IQRCodeFormat
    {
        protected ILogger<QRCodeFormat> Logger { get; }
        protected IUmbracoHelperAccessor UmbracoHelperAccessor { get; }
        protected IQRCodeHashIdFactory HashIdFactory { get; }
        protected IQRCodeType CodeType { get; }
        protected QRCodeSettings Settings { get; }

        public QRCodeFormat(IUmbracoHelperAccessor umbracoHelperAccessor, IQRCodeHashIdFactory hashIdFactory, ILogger<QRCodeFormat> logger, IQRCodeType codeType,
            QRCodeSettings settings)
        {
            UmbracoHelperAccessor = umbracoHelperAccessor;
            HashIdFactory = hashIdFactory;
            CodeType = codeType;
            Settings = settings;
            Logger = logger;
        }

        public abstract string Mime { get; }

        public virtual string FileName => $"{HashId}.{FileExtension}";

        public abstract string FileExtension { get; }

        public virtual string HashId => HashIdFactory.ComputeHash(CodeType.GetCodeContent(), Settings);

        public abstract Stream Stream();

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
                    return GetUmbracoHelper()?.Media(mediaId)?.Url();
                }
                if (UdiParser.TryParse(icon, out Udi mediaUdi))
                {
                    return GetUmbracoHelper()?.Media(mediaUdi)?.Url();
                }
                return icon;
            }
            return null;
        }

        private UmbracoHelper GetUmbracoHelper()
        {
            if (UmbracoHelperAccessor.TryGetUmbracoHelper(out var umbracoHelper))
            {
                return umbracoHelper;
            }
            Logger.LogWarning("Umbraco Helper not found.");
            return null;
        }
    }
}