using DotNetColorParser;
using RoboLynx.Umbraco.QRCodeGenerator.Helpers;
using RoboLynx.Umbraco.QRCodeGenerator.Models;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using Umbraco.Core.IO;
using Umbraco.Core.Logging;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat
{
    public class PngFormat : RasterFormat
    {
        readonly ImageFormat _imageFormat = ImageFormat.Png;

        public PngFormat(IMediaService mediaService, IUmbracoHelperAccessor umbracoHelperAccessor, IQRCodeHashIdFactory hashIdFactory,
            ILogger logger, IColorParser colorParser, IQRCodeType codeType, QRCodeSettings settings) 
                : base(mediaService, umbracoHelperAccessor, hashIdFactory, logger, colorParser, codeType, settings)
        {

        }

        public override string Mime => "image/png";

        public override string FileExtension => "png";

        public override Stream Stream()
        {
            return RasterStream(_imageFormat);
        }
    }
}
