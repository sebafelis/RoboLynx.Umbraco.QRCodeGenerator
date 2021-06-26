using DotNetColorParser;
using RoboLynx.Umbraco.QRCodeGenerator.Models;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes;
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
        readonly ImageFormat _imageFormat = ImageFormat.Bmp;

        public BmpFormat(IMediaFileSystem mediaFileSystem, UmbracoHelper umbracoHelper, IQRCodeHashIdFactory hashIdFactory, IColorParser colorParser, IQRCodeType codeType, QRCodeSettings settings) : base(mediaFileSystem, umbracoHelper, hashIdFactory, colorParser, codeType, settings)
        {

        }

        public override string Mime => "image/bmp";

        public override string FileExtension => "bmp";

        public override Stream Stream()
        {
            return RasterStream(_imageFormat);
        }
    }
}
