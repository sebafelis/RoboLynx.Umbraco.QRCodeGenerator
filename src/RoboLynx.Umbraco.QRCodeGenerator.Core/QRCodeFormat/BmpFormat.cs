using DotNetColorParser;
using Microsoft.Extensions.Logging;
using RoboLynx.Umbraco.QRCodeGenerator.Models;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes;
using System.Drawing.Imaging;
using System.IO;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Web.Common;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat
{
    public class BmpFormat : RasterFormat
    {
        private readonly ImageFormat _imageFormat = ImageFormat.Bmp;

        public BmpFormat(IMediaService mediaService, IUmbracoHelperAccessor umbracoHelperAccessor,
            IQRCodeHashIdFactory hashIdFactory, IColorParser colorParser, ILogger<BmpFormat> logger,
            IQRCodeType codeType, QRCodeSettings settings)
                : base(mediaService, umbracoHelperAccessor, hashIdFactory, logger, colorParser, codeType, settings)
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