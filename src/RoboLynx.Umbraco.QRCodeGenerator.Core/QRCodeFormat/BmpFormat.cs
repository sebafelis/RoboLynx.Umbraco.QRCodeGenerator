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
    public class BmpFormat : RasterFormat
    {
        public BmpFormat(IMediaFileSystem mediaFileSystem, UmbracoHelper umbracoHelper, IColorParser colorParser, string codeContent, QRCodeSettings settings) : base(mediaFileSystem, umbracoHelper, colorParser, codeContent, settings)
        {

        }

        ImageFormat _imageFormat = ImageFormat.Bmp;

        public override string Id => "bmp";

        public override string Mime => "image/bmp";

        public override string FileExtension => "bmp";

        public override Stream Stream()
        {
            return RasterStream(value, settings, _imageFormat);
        }
    }
}
