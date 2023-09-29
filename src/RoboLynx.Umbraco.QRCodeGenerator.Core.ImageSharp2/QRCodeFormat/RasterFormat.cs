using DotNetColorParser;
using Microsoft.Extensions.Logging;
using QRCoder;
using RoboLynx.Umbraco.QRCodeGenerator.Models;
using RoboLynx.Umbraco.QRCodeGenerator.ImageSharp;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using System.IO;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Web.Common;
using System.Reflection;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat
{
    public abstract class RasterFormat : QRCodeFormat
    {
        private readonly IMediaService _mediaService;
        private readonly IColorParser _colorParser;

        public RasterFormat(IMediaService mediaService, IUmbracoHelperAccessor umbracoHelperAccessor, IQRCodeHashIdFactory hashIdFactory, ILogger<RasterFormat> logger, IColorParser colorParser, IQRCodeType codeType, QRCodeSettings Settings) : base(umbracoHelperAccessor, hashIdFactory, logger, codeType, Settings)
        {
            _mediaService = mediaService;
            _colorParser = colorParser;

            var version = Assembly.GetAssembly(typeof(SixLabors.ImageSharp.Image)).GetName().Version;
            logger.LogInformation("ImageSharp library in version {version} is use.", version);

		}

        protected MemoryStream RasterStream(IImageEncoder imageFormat)
        {
            var lightColor = _colorParser.ParseColor(Settings.LightColor).ToImageSharpColor();
            var darkColor = _colorParser.ParseColor(Settings.DarkColor).ToImageSharpColor();

            using var qrCodeBmp = GenerateBitmapQRCode(CodeType.GetCodeContent(), Settings.Size ?? DefaultFieldsValues.DefaultSizeFieldValue, darkColor, lightColor, Settings.DrawQuiteZone ?? DefaultFieldsValues.DefaultDrawQuietZoneFieldValue, ResolveIconUrl(Settings.Icon), Settings.IconSizePercent ?? DefaultFieldsValues.DefaultIconSizePercentFieldValue, Settings.IconBorderWidth ?? DefaultFieldsValues.DefaultIconBorderWidthFieldValue, Settings.ECCLevel ?? DefaultFieldsValues.DefaultECCLevelFieldValue);

            MemoryStream memoryStream = new();
            qrCodeBmp.Save(memoryStream, imageFormat);
            memoryStream.Seek(0, SeekOrigin.Begin);

            return memoryStream;
        }

        private Image GenerateBitmapQRCode(string codeContent, int size, Color darkColor, Color lightColor, bool drawQuiteZone, string? iconUrl, int iconSizePercent, int iconBorderWidth, ECCLevel level)
        {
            var qrGenerator = new QRCoder.QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(codeContent, (QRCoder.QRCodeGenerator.ECCLevel)((int)level), true);

            ImageSharpQRCode bmpQrCode = new(qrCodeData);
            if (!string.IsNullOrEmpty(iconUrl))
            {
                using var iconStream = _mediaService.GetMediaFileContentStream(iconUrl);
                using var iconBmp =  Image.Load(iconStream);
                if (iconBmp is not null)
                {
                    return bmpQrCode.GetGraphic(size, darkColor, lightColor, iconBmp, iconSizePercent, iconBorderWidth, drawQuiteZone);
                }
            }
            return bmpQrCode.GetGraphic(size, darkColor, lightColor, drawQuiteZone);
        }
    }
}