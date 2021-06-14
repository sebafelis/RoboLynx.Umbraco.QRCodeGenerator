using DotNetColorParser;
using QRCoder;
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
        private readonly IMediaFileSystem _mediaFileSystem;
        private readonly IColorParser _colorParser;

        public RasterFormat(IMediaFileSystem mediaFileSystem, UmbracoHelper umbracoHelper, IColorParser colorParser, string codeContent, QRCodeSettings settings) : base(umbracoHelper, codeContent, settings)
        {
            _mediaFileSystem = mediaFileSystem;
            _colorParser = colorParser;
        }

        protected MemoryStream RasterStream(string value, QRCodeSettings settings, ImageFormat imageFormat)
        {
            var lightColor = _colorParser.ParseColor(settings.LightColor);
            var darkColor = _colorParser.ParseColor(settings.DarkColor);

            using var qrCodeBmp = GenerateBitmapQRCode(value, settings.Size, darkColor, lightColor, settings.DrawQuiteZone.Value, ResolveIconUrl(settings.Icon), settings.IconSizePercent, settings.IconBorderWidth.Value, settings.ECCLevel.Value);

            using MemoryStream ms = new();
            qrCodeBmp.Save(ms, imageFormat);

            return ms;
        }

        private Bitmap GenerateBitmapQRCode(string value, int size, Color darkColor, Color lightColor, bool drawQuiteZone, string iconUrl, int iconSizePercent, int iconBorderWidth, ECCLevel level)
        {
            var qrGenerator = new QRCoder.QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(value, (QRCoder.QRCodeGenerator.ECCLevel)((int)level), true);

            QRCode bmpQrCode = new(qrCodeData);
            if (!string.IsNullOrEmpty(iconUrl))
            {
                using var iconStream = _mediaFileSystem.OpenFile(iconUrl);
                using var iconBmp = new Bitmap(iconStream);
                if (!(iconBmp is null))
                {
                    return bmpQrCode.GetGraphic(size, darkColor, lightColor, iconBmp, iconSizePercent, iconBorderWidth, drawQuiteZone);
                }
            }
            return bmpQrCode.GetGraphic(size, darkColor, lightColor, drawQuiteZone);
        }
    }
}
