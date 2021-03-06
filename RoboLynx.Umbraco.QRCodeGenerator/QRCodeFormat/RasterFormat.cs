using QRCoder;
using RoboLynx.Umbraco.QRCodeGenerator.Controllers;
using RoboLynx.Umbraco.QRCodeGenerator.Models;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using Umbraco.Core.IO;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat
{
    public abstract class RasterFormat : QRCodeFormat
    {
        private readonly IMediaFileSystem mediaFileSystem;

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

        public RasterFormat(ILocalizedTextService localizedTextService, IMediaFileSystem mediaFileSystem, UmbracoHelper umbracoHelper) : base(localizedTextService, umbracoHelper)
        {
            this.mediaFileSystem = mediaFileSystem;
        }

        protected HttpContent RasterResponseContent(string value, QRCodeSettings settings, ImageFormat imageFormat)
        {
            var lightColor = ColorTranslator.FromHtml(settings.LightColor);
            var darkColor = ColorTranslator.FromHtml(settings.DarkColor);

            using var qrCodeBmp = GenerateBitmapQRCode(value, settings.Size, darkColor, lightColor, settings.DrawQuiteZone.Value, ResolveIconUrl(settings.Icon), settings.IconSizePercent, settings.IconBorderWidth.Value, settings.ECCLevel.Value);
            return SetBitmapAsHttpContent(qrCodeBmp, imageFormat, Mime, FileName);
        }

        private Bitmap GenerateBitmapQRCode(string value, int size, Color darkColor, Color lightColor, bool drawQuiteZone, string iconUrl, int iconSizePercent, int iconBorderWidth, ECCLevel level)
        {
            var qrGenerator = new QRCoder.QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(value, (QRCoder.QRCodeGenerator.ECCLevel)((int)level), true);

            QRCode bmpQrCode = new QRCode(qrCodeData);
            if (!string.IsNullOrEmpty(iconUrl))
            {
                using var iconStream = mediaFileSystem.OpenFile(iconUrl);
                using var iconBmp = new Bitmap(iconStream);
                if (!(iconBmp is null))
                {
                    return bmpQrCode.GetGraphic(size, darkColor, lightColor, iconBmp, iconSizePercent, iconBorderWidth, drawQuiteZone);
                }
            }
            return bmpQrCode.GetGraphic(size, darkColor, lightColor, drawQuiteZone);
        }

        private HttpContent SetBitmapAsHttpContent(Bitmap bitmap, ImageFormat format, string mime, string fileName)
        {
            using MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, format);

            var httpContent = new ByteArrayContent(ms.ToArray());
            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(mime);
            if (!string.IsNullOrEmpty(fileName))
            {
                httpContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = fileName
                };
            }

            return httpContent;
        }
    }
}
