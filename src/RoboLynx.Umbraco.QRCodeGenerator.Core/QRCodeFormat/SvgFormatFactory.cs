using DotNetColorParser;
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
    public class SvgFormatFactory : QRCodeFormatFactory
    {
        private readonly IUmbracoHelperAccessor _umbracoHelperAccessor;
        private readonly IQRCodeHashIdFactory _hashIdFactory;
        private readonly IColorParser _colorParser;
        private readonly ILogger _logger;

        public SvgFormatFactory(ILocalizedTextService localizedTextService, IUmbracoHelperAccessor umbracoHelperAccessor, 
            IQRCodeHashIdFactory hashIdFactory, IColorParser colorParser, ILogger logger) : base(localizedTextService)
        {
            _umbracoHelperAccessor = umbracoHelperAccessor;
            _hashIdFactory = hashIdFactory;
            _colorParser = colorParser;
            _logger = logger;
        }

        public override string Id => "svg";

        public override IEnumerable<string> RequiredSettings => new List<string> {
            Constants.FieldsNames.SizeFieldName,
            Constants.FieldsNames.FormatFieldName,
            Constants.FieldsNames.DarkColorFieldName,
            Constants.FieldsNames.LightColorFieldName,
            Constants.FieldsNames.DrawQuietZoneFieldName,
            Constants.FieldsNames.ECCLevelFieldName
        };

        public override IQRCodeFormat Create(IQRCodeType codeType, QRCodeSettings settings)
        {
            return new SvgFormat(_umbracoHelperAccessor, _hashIdFactory, _colorParser, _logger, codeType, settings);
        }
    }
}
