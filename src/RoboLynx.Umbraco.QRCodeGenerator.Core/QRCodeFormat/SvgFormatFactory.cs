using DotNetColorParser;
using RoboLynx.Umbraco.QRCodeGenerator.Models;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using Umbraco.Core.IO;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat
{
    public class SvgFormatFactory : QRCodeFormatFactory
    {
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IQRCodeHashIdFactory _hashIdFactory;
        private readonly IColorParser _colorParser;

        public SvgFormatFactory(ILocalizedTextService localizedTextService, UmbracoHelper umbracoHelper, IQRCodeHashIdFactory hashIdFactory, IColorParser colorParser) : base(localizedTextService)
        {
            _umbracoHelper = umbracoHelper;
            _hashIdFactory = hashIdFactory;
            _colorParser = colorParser;
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
            return new SvgFormat(_umbracoHelper, _hashIdFactory, _colorParser, codeType, settings);
        }
    }
}
