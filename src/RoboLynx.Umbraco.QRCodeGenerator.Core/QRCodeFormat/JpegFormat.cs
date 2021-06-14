using DotNetColorParser;
using RoboLynx.Umbraco.QRCodeGenerator.Models;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using Umbraco.Core.IO;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat
{
    public class JpegFormat : RasterFormat
    {
        public JpegFormat(ILocalizedTextService localizedTextService, IMediaFileSystem mediaFileSystem, UmbracoHelper umbracoHelper, IColorParser colorParser) : base(localizedTextService, mediaFileSystem, umbracoHelper, colorParser)
        {

        }

        ImageFormat imageFormat = ImageFormat.Jpeg;

        public override string Id => "jpeg";

        public override string Mime => "image/jpeg";

        public override string FileExtension => "jpg";

        public override Stream Stream(string value, QRCodeSettings settings)
        {
            return RasterStream(value, settings, imageFormat);
        }
    }
}
