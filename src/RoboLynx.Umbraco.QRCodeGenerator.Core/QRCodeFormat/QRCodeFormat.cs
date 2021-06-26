using RoboLynx.Umbraco.QRCodeGenerator.Models;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes;
using System.IO;
using Umbraco.Core;
using Umbraco.Web;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat
{
    public abstract class QRCodeFormat : IQRCodeFormat
    {
        protected UmbracoHelper UmbracoHelper { get; }
        protected IQRCodeHashIdFactory HashIdFactory { get; }
        protected IQRCodeType CodeType { get; }
        protected QRCodeSettings Settings { get; }

        public QRCodeFormat(UmbracoHelper umbracoHelper, IQRCodeHashIdFactory hashIdFactory, IQRCodeType codeType, QRCodeSettings settings)
        {
            UmbracoHelper = umbracoHelper;
            HashIdFactory = hashIdFactory;
            CodeType = codeType;
            Settings = settings;
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
                    return UmbracoHelper.Media(mediaId)?.Url();
                }
                if (Udi.TryParse(icon, out Udi mediaUdi))
                {
                    return UmbracoHelper.Media(mediaUdi)?.Url();
                }
                return icon;
            }
            return null;
        }
    }
}
