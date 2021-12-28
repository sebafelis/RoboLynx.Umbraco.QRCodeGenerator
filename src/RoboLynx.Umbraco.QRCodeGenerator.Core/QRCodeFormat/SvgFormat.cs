using DotNetColorParser;
using QRCoder;
using RoboLynx.Umbraco.QRCodeGenerator.Exceptions;
using RoboLynx.Umbraco.QRCodeGenerator.Helpers;
using RoboLynx.Umbraco.QRCodeGenerator.Models;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Umbraco.Core.Logging;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat
{
    public class SvgFormat : QRCodeFormat
    {
        private readonly IColorParser _colorParser;

        public SvgFormat(IUmbracoHelperAccessor _umbracoHelperAccessor, IQRCodeHashIdFactory hashIdFactory, IColorParser colorParser, ILogger logger,  IQRCodeType codeType, QRCodeSettings settings) : base(_umbracoHelperAccessor, hashIdFactory, logger, codeType, settings)
        {
            _colorParser = colorParser;
        }
        

        public override string Mime => "image/svg+xml";

        public override string FileExtension => "svg";

        public override Stream Stream()
        {
            var svgString = CreateSvgCode(CodeType.GetCodeContent(), Settings);

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream, Encoding.UTF8);
            writer.Write(svgString);
            writer.Flush();
            stream.Seek(0, SeekOrigin.Begin);

            return stream;
        }

        private string CreateSvgCode(string codeContent, QRCodeSettings settings)
        {
            var lightColor = _colorParser.ParseColor(settings.LightColor);
            var darkColor = _colorParser.ParseColor(settings.DarkColor);

            var qrGenerator = new QRCoder.QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(codeContent, (QRCoder.QRCodeGenerator.ECCLevel)(int)settings.ECCLevel, true);

            SvgQRCode svgQrCode = new (qrCodeData);

            var svgString = svgQrCode.GetGraphic(settings.Size, darkColor, lightColor, settings.DrawQuiteZone.Value, SvgQRCode.SizingMode.WidthHeightAttribute);

            svgString = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>\n<!DOCTYPE svg PUBLIC \"-//W3C//DTD SVG 1.1//EN\"\n\"http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd\"> \n" + svgString;

            return svgString;
        }


    }
}
