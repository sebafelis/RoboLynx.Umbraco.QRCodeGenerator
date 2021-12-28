using DotNetColorParser;
using QRCoder;
using RoboLynx.Umbraco.QRCodeGenerator.Models;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes;
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
        private readonly IMediaService _mediaService;
        private readonly IColorParser _colorParser;

        public RasterFormat(IMediaService mediaService, UmbracoHelper umbracoHelper, IQRCodeHashIdFactory hashIdFactory, IColorParser colorParser, IQRCodeType codeType, QRCodeSettings Settings) : base(umbracoHelper, hashIdFactory, codeType, Settings)
        {
            _mediaService = mediaService;
            _colorParser = colorParser;
        }

        protected MemoryStream RasterStream(ImageFormat imageFormat)
        {
            var lightColor = _colorParser.ParseColor(Settings.LightColor);
            var darkColor = _colorParser.ParseColor(Settings.DarkColor);

            using var qrCodeBmp = GenerateBitmapQRCode(CodeType.GetCodeContent(), Settings.Size, darkColor, lightColor, Settings.DrawQuiteZone.Value, ResolveIconUrl(Settings.Icon), Settings.IconSizePercent, Settings.IconBorderWidth.Value, Settings.ECCLevel.Value);

            MemoryStream memoryStream = new();
            qrCodeBmp.Save(memoryStream, imageFormat);
            memoryStream.Seek(0, SeekOrigin.Begin);

            return memoryStream;
        }

        private Bitmap GenerateBitmapQRCode(string codeContent, int size, Color darkColor, Color lightColor, bool drawQuiteZone, string iconUrl, int iconSizePercent, int iconBorderWidth, ECCLevel level)
        {
            var qrGenerator = new QRCoder.QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(codeContent, (QRCoder.QRCodeGenerator.ECCLevel)((int)level), true);

            QRCode bmpQrCode = new(qrCodeData);
            if (!string.IsNullOrEmpty(iconUrl))
            {
                using var iconStream = _mediaService.GetMediaFileContentStream(iconUrl);
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
