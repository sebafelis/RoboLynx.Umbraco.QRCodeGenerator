using DotNetColorParser;
using RoboLynx.Umbraco.QRCodeGenerator.Models;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using Umbraco.Core.IO;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat
{
    public class BmpFormatFactory : QRCodeFormatFactory
    {
        public BmpFormatFactory(ILocalizedTextService localizedTextService, IMediaFileSystem mediaFileSystem, UmbracoHelper umbracoHelper, IColorParser colorParser) : base(localizedTextService, mediaFileSystem, umbracoHelper, colorParser)
        {

        }

        public override string Id => "bmp";

        public override IEnumerable<string> RequiredSettings => new List<string> {
            Constants.SizeFieldName,
            Constants.FormatFieldName,
            Constants.DarkColorFieldName,
            Constants.LightColorFieldName,
            Constants.DrawQuietZoneFieldName,
            Constants.IconBorderWidthFieldName,
            Constants.IconFieldName,
            Constants.IconSizePercentFieldName,
            Constants.ECCLevelFieldName
        };

        public override IQRCodeFormat Create(string codeContent, QRCodeSettings settings)
        {
            return new BmpFormat()
        }
    }
}
