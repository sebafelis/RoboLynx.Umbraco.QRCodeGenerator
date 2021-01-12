using QRCoder;
using RoboLynx.Umbraco.QRCodeGenerator.Controllers;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using Umbraco.Core.IO;
using Umbraco.Web;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat
{
    public abstract class RasterFormat : QRCodeFormat
    {
        public override IEnumerable<string> RequiredSettings => new List<string> { 
            Constants.SizeFieldName,
            Constants.FormatFieldName,
            Constants.DarkColorFieldName,
            Constants.LightColorFieldName,
            Constants.DrawQuietZoneFieldName,
            Constants.IconBorderWidthFieldName,
            Constants.IconFieldName,
            Constants.IconSizePercentFieldName
        };

        protected HttpContent RasterResponseContent(string value, QRCodeSettings settings, UmbracoHelper umbracoHelper, ImageFormat imageFormat, string mime)
        {
            var lightColor = ColorTranslator.FromHtml(settings.LightColor);
            var darkColor = ColorTranslator.FromHtml(settings.DarkColor);

            var qrGenerator = new QRCoder.QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(value, QRCoder.QRCodeGenerator.ECCLevel.Q);

            var qrCodeBmp = GenerateBitmapQRCode(value, settings.Size, darkColor, lightColor, settings.DrawQuiteZone.Value, ResolveIconUrl(settings.Icon, umbracoHelper), settings.IconSizePercent, settings.IconBorderWidth.Value);
            return SetBitmapAsHttpContent(qrCodeBmp, imageFormat, mime, FileName);
        }

        private Bitmap GenerateBitmapQRCode(string value, int size, Color darkColor, Color lightColor, bool drawQuiteZone, string iconUrl, int iconSizePercent, int iconBorderWidth)
        {
            var qrGenerator = new QRCoder.QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(value, QRCoder.QRCodeGenerator.ECCLevel.Q);

            QRCode bmpQrCode = new QRCode(qrCodeData);
            Bitmap qrCodeBmp = null;
            if (!string.IsNullOrEmpty(iconUrl))
            {
                using (var iconStream = FileSystemProviderManager.Current.MediaFileSystem.OpenFile(iconUrl))
                {
                    using (var iconBmp = new Bitmap(iconStream))
                    {
                        qrCodeBmp = bmpQrCode.GetGraphic(size, darkColor, lightColor, iconBmp, iconSizePercent, iconBorderWidth, drawQuiteZone);
                    }
                }
            }

            if (qrCodeBmp == null)
            {
                qrCodeBmp = bmpQrCode.GetGraphic(size, darkColor, lightColor, drawQuiteZone);
            }

            return qrCodeBmp;
        }

        private HttpContent SetBitmapAsHttpContent(Bitmap bitmap, ImageFormat format, string mime, string fileName)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, format);

                var httpContent = new ByteArrayContent(ms.ToArray());
                httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(mime);
                if (!string.IsNullOrEmpty(fileName))
                {
                    httpContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                    httpContent.Headers.ContentDisposition.FileName = fileName;
                }

                return httpContent;
            }
        }
    }
}
