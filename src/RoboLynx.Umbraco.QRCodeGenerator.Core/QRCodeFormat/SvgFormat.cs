using DotNetColorParser;
using QRCoder;
using RoboLynx.Umbraco.QRCodeGenerator.Exceptions;
using RoboLynx.Umbraco.QRCodeGenerator.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat
{
    public class SvgFormat : QRCodeFormat
    {
        private readonly IColorParser colorParser;

        public SvgFormat(ILocalizedTextService localizedTextService, UmbracoHelper umbracoHelper, IColorParser colorParser) : base(localizedTextService, umbracoHelper)
        {
            this.colorParser = colorParser;
        }

        public override IEnumerable<string> RequiredSettings => new List<string> {
            Constants.SizeFieldName,
            Constants.FormatFieldName,
            Constants.DarkColorFieldName,
            Constants.LightColorFieldName,
            Constants.DrawQuietZoneFieldName,
            Constants.ECCLevelFieldName
        };

        public override string Id => "svg";

        public override string Mime => "image/svg+xml";

        public override string FileExtension => "svg";

        public override Stream Stream(string value, QRCodeSettings settings)
        {
            var svgString = CreateSvgCode(value, settings);

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream, Encoding.UTF8);
            writer.Write(svgString);
            writer.Flush();
            stream.Position = 0;

            return stream;
        }

        private string CreateSvgCode(string value, QRCodeSettings settings)
        {
            var lightColor = colorParser.ParseColor(settings.LightColor);
            var darkColor = colorParser.ParseColor(settings.DarkColor);

            var qrGenerator = new QRCoder.QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(value, (QRCoder.QRCodeGenerator.ECCLevel)(int)settings.ECCLevel, true);

            SvgQRCode svgQrCode = new (qrCodeData);

            var svgString = svgQrCode.GetGraphic(settings.Size, darkColor, lightColor, settings.DrawQuiteZone.Value, SvgQRCode.SizingMode.WidthHeightAttribute);

            svgString = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>\n<!DOCTYPE svg PUBLIC \"-//W3C//DTD SVG 1.1//EN\"\n\"http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd\"> \n" + svgString;

            return svgString;
        }


    }
}
