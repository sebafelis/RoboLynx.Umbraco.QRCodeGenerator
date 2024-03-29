﻿using DotNetColorParser;
using Microsoft.Extensions.Logging;
using RoboLynx.Umbraco.QRCodeGenerator.Models;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes;
using System.IO;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Web.Common;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat
{
    public class PngFormat : RasterFormat
    {
        private readonly SixLabors.ImageSharp.Formats.IImageEncoder _imageFormat = new SixLabors.ImageSharp.Formats.Png.PngEncoder();

        public PngFormat(IMediaService mediaService, IUmbracoHelperAccessor umbracoHelperAccessor, IQRCodeHashIdFactory hashIdFactory,
            ILogger<PngFormat> logger, IColorParser colorParser, IQRCodeType codeType, QRCodeSettings settings)
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