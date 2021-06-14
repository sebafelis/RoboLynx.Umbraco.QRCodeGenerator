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
    public class PngFormat : RasterFormat
    {
        public PngFormat(ILocalizedTextService localizedTextService, IMediaFileSystem mediaFileSystem, UmbracoHelper umbracoHelper, IColorParser colorParser) : base(localizedTextService, mediaFileSystem, umbracoHelper, colorParser)
        {

        }

        ImageFormat imageFormat = ImageFormat.Png;

        public override string Id => "png";

        public override string Mime => "image/png";

        public override string FileExtension => "png";

        public override Stream Stream(string value, QRCodeSettings settings)
        {
            return RasterStream(value, settings, imageFormat);
        }
    }
}
