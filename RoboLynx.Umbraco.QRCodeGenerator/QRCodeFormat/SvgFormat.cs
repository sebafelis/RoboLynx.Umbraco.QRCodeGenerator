using QRCoder;
using RoboLynx.Umbraco.QRCodeGenerator.Controllers;
using RoboLynx.Umbraco.QRCodeGenerator.Models;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat
{
    public class SvgFormat : QRCodeFormat
    {
        public SvgFormat(ILocalizedTextService localizedTextService, UmbracoHelper umbracoHelper) : base(localizedTextService, umbracoHelper)
        {

        }

        public override string Id => "svg";

        public override IEnumerable<string> RequiredSettings => new List<string> {
            Constants.SizeFieldName,
            Constants.FormatFieldName,
            Constants.DarkColorFieldName,
            Constants.LightColorFieldName,
            Constants.DrawQuietZoneFieldName,
            Constants.ECCLevelFieldName
        };

        public override string FileName => base.FileName + ".svg";

        public override string Mime => "image/svg+xml";

        public override HttpContent ResponseContent(string value, QRCodeSettings settings)
        {
            var lightColor = ColorTranslator.FromHtml(settings.LightColor);
            var darkColor = ColorTranslator.FromHtml(settings.DarkColor);

            var qrGenerator = new QRCoder.QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(value, (QRCoder.QRCodeGenerator.ECCLevel)(int)settings.ECCLevel, true);
            SvgQRCode svgQrCode = new SvgQRCode(qrCodeData);
            var svgString = svgQrCode.GetGraphic(settings.Size, darkColor, lightColor, settings.DrawQuiteZone.Value, SvgQRCode.SizingMode.WidthHeightAttribute);
            svgString = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>\n<!DOCTYPE svg PUBLIC \"-//W3C//DTD SVG 1.1//EN\"\n\"http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd\"> \n" + svgString;

            var httpContent = new StringContent(svgString, Encoding.UTF8);

            httpContent.Headers.ContentType = new MediaTypeHeaderValue(Mime)
            {
                CharSet = Encoding.UTF8.HeaderName
            };
            if (!string.IsNullOrEmpty(FileName))
            {
                httpContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = FileName
                };
            }

            return httpContent;
        }
    }
}
