using DotNetColorParser;
using Microsoft.Extensions.Logging;
using RoboLynx.Umbraco.QRCodeGenerator.Models;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes;
using System.Collections.Generic;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Web.Common;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat
{
    public class JpegFormatFactory : QRCodeFormatFactory
    {
        private readonly IMediaService _mediaService;
        private readonly IUmbracoHelperAccessor _umbracoHelperAccessor;
        private readonly IQRCodeHashIdFactory _hashIdFactory;
        private readonly IColorParser _colorParser;
        private readonly ILogger<JpegFormat> _logger;

        public JpegFormatFactory(ILocalizedTextService localizedTextService, IMediaService mediaService,
            IUmbracoHelperAccessor umbracoHelperAccessor, IQRCodeHashIdFactory hashIdFactory,
            IColorParser colorParser, ILogger<JpegFormat> logger) : base(localizedTextService)
        {
            _mediaService = mediaService;
            _umbracoHelperAccessor = umbracoHelperAccessor;
            _hashIdFactory = hashIdFactory;
            _colorParser = colorParser;
            _logger = logger;
        }

        public override string Id => "jpeg";

        public override IEnumerable<string> RequiredSettings => new List<string> {
            SettingProperties.Size,
            SettingProperties.DarkColor,
            SettingProperties.LightColor,
            SettingProperties.DrawQuietZone,
            SettingProperties.IconBorderWidth,
            SettingProperties.Icon,
            SettingProperties.IconSizePercent,
            SettingProperties.ECCLevel
        };

        public override IQRCodeFormat Create(IQRCodeType codeType, QRCodeSettings settings)
        {
            return new JpegFormat(_mediaService, _umbracoHelperAccessor, _hashIdFactory, _colorParser, _logger, codeType, settings);
        }
    }
}