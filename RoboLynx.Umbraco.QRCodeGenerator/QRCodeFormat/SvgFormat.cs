using QRCoder;
using RoboLynx.Umbraco.QRCodeGenerator.Controllers;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Umbraco.Web;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat
{
    public class SvgFormat : QRCodeFormat
    {
        public override string Id => "svg";

        public override IEnumerable<string> RequiredSettings => new List<string> {
            Constants.SizeFieldName,
            Constants.FormatFieldName,
            Constants.DarkColorFieldName,
            Constants.LightColorFieldName,
            Constants.DrawQuietZoneFieldName
        };

        public override string FileName => base.FileName + ".svg";

        public override HttpContent ResponseContent(string value, QRCodeSettings settings, UmbracoHelper umbracoHelper)
        {
            var lightColor = ColorTranslator.FromHtml(settings.LightColor);
            var darkColor = ColorTranslator.FromHtml(settings.DarkColor);

            var qrGenerator = new QRCoder.QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(value, QRCoder.QRCodeGenerator.ECCLevel.Q);
            SvgQRCode svgQrCode = new SvgQRCode(qrCodeData);
            var svgString = svgQrCode.GetGraphic(settings.Size, darkColor, lightColor, settings.DrawQuiteZone.Value, SvgQRCode.SizingMode.WidthHeightAttribute);
            svgString = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>\n<!DOCTYPE svg PUBLIC \"-//W3C//DTD SVG 1.1//EN\"\n\"http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd\"> \n" + svgString;

            var httpContent = new StringContent(svgString, Encoding.UTF8);

            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/svg+xml");
            httpContent.Headers.ContentType.CharSet = Encoding.UTF8.HeaderName;
            if (!string.IsNullOrEmpty(FileName))
            {
                httpContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                httpContent.Headers.ContentDisposition.FileName = FileName;
            }

            return httpContent;
        }
    }
}
