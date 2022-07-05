using DotNetColorParser;
using Microsoft.Extensions.Logging;
using RoboLynx.Umbraco.QRCodeGenerator.Models;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes;
using System.IO;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Web.Common;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat
{
    public class JpegFormat : RasterFormat
    {
        private readonly SixLabors.ImageSharp.Formats.IImageEncoder _imageFormat = new SixLabors.ImageSharp.Formats.Jpeg.JpegEncoder() { Quality = 80 };

        public JpegFormat(IMediaService mediaService, IUmbracoHelperAccessor umbracoHelperAccessor,
            IQRCodeHashIdFactory hashIdFactory, IColorParser colorParser, ILogger<JpegFormat> logger,
            IQRCodeType codeType, QRCodeSettings settings)
                : base(mediaService, umbracoHelperAccessor, hashIdFactory, logger, colorParser, codeType, settings)
        {
        }

        public override string Mime => "image/jpeg";

        public override string FileExtension => "jpg";

        public override Stream Stream()
        {
            return RasterStream(_imageFormat);
        }
    }
}