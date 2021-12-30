using RoboLynx.Umbraco.QRCodeGenerator.Helpers;
using RoboLynx.Umbraco.QRCodeGenerator.Models;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes;
using Serilog.Core;
using System.IO;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;
using Umbraco.Web.PublishedCache;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat
{
    public abstract class QRCodeFormat : IQRCodeFormat
    {
        protected ILogger Logger { get; }
        protected IUmbracoHelperAccessor UmbracoHelperAccessor { get; }
        protected IQRCodeHashIdFactory HashIdFactory { get; }
        protected IQRCodeType CodeType { get; }
        protected QRCodeSettings Settings { get; }

        public QRCodeFormat(IUmbracoHelperAccessor umbracoHelperAccessor, IQRCodeHashIdFactory hashIdFactory, ILogger logger, IQRCodeType codeType, 
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
                if (Udi.TryParse(icon, out Udi mediaUdi))
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
            Logger.Warn<QRCodeFormat>("Umbraco Helper not found.");
            return null;
        }
}
}
