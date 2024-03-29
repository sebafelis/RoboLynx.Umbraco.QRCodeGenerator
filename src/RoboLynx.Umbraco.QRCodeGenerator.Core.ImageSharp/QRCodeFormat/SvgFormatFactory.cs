﻿using DotNetColorParser;
using Microsoft.Extensions.Logging;
using RoboLynx.Umbraco.QRCodeGenerator.Models;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes;
using System.Collections.Generic;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Web.Common;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat
{
    public class SvgFormatFactory : QRCodeFormatFactory
    {
        private readonly IUmbracoHelperAccessor _umbracoHelperAccessor;
        private readonly IQRCodeHashIdFactory _hashIdFactory;
        private readonly IColorParser _colorParser;
        private readonly ILogger<SvgFormat> _logger;

        public SvgFormatFactory(ILocalizedTextService localizedTextService, IUmbracoHelperAccessor umbracoHelperAccessor,
            IQRCodeHashIdFactory hashIdFactory, IColorParser colorParser, ILogger<SvgFormat> logger) : base(localizedTextService)
        {
            _umbracoHelperAccessor = umbracoHelperAccessor;
            _hashIdFactory = hashIdFactory;
            _colorParser = colorParser;
            _logger = logger;
        }

        public override string Id => "svg";

        public override IEnumerable<string> RequiredSettings => new List<string> {
            SettingProperties.Size,
            SettingProperties.DarkColor,
            SettingProperties.LightColor,
            SettingProperties.DrawQuietZone,
            SettingProperties.ECCLevel
        };

        public override IQRCodeFormat Create(IQRCodeType codeType, QRCodeSettings settings)
        {
            return new SvgFormat(_umbracoHelperAccessor, _hashIdFactory, _colorParser, _logger, codeType, settings);
        }
    }
}