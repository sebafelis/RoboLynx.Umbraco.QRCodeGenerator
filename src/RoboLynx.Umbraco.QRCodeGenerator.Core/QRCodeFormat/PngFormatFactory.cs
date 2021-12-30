﻿using DotNetColorParser;
using RoboLynx.Umbraco.QRCodeGenerator.Helpers;
using RoboLynx.Umbraco.QRCodeGenerator.Models;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using Umbraco.Core.IO;
using Umbraco.Core.Logging;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat
{
    public class PngFormatFactory : QRCodeFormatFactory
    {
        private readonly IMediaService _mediaService;
        private readonly IUmbracoHelperAccessor _umbracoHelperAccessor;
        private readonly IQRCodeHashIdFactory _hashIdFactory;
        private readonly IColorParser _colorParser;
        private readonly ILogger _logger;

        public PngFormatFactory(ILocalizedTextService localizedTextService, IMediaService mediaService,
            IUmbracoHelperAccessor umbracoHelperAccessor, IQRCodeHashIdFactory hashIdFactory,
            IColorParser colorParser, ILogger logger) : base(localizedTextService)
        {
            _mediaService = mediaService;
            _umbracoHelperAccessor = umbracoHelperAccessor;
            _hashIdFactory = hashIdFactory;
            _colorParser = colorParser;
            _logger = logger;
        }

        public override string Id => "png";

        public override IEnumerable<string> RequiredSettings => new List<string> {
            Constants.SettingProperties.Size,
            Constants.SettingProperties.DarkColor,
            Constants.SettingProperties.LightColor,
            Constants.SettingProperties.DrawQuietZone,
            Constants.SettingProperties.IconBorderWidth,
            Constants.SettingProperties.Icon,
            Constants.SettingProperties.IconSizePercent,
            Constants.SettingProperties.ECCLevel
        };

        public override IQRCodeFormat Create(IQRCodeType codeType, QRCodeSettings settings)
        {
            return new PngFormat(_mediaService, _umbracoHelperAccessor, _hashIdFactory, _logger, _colorParser, codeType, settings);
        }
    }
}
