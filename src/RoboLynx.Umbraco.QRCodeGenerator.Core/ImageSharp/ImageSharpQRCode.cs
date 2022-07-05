using QRCoder;
using System;
using SixLabors.ImageSharp;
using static QRCoder.QRCodeGenerator;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Drawing;

namespace RoboLynx.Umbraco.QRCodeGenerator.ImageSharp
{
    public class ImageSharpQRCode : AbstractQRCode, IDisposable
    {
        /// <summary>
        /// Constructor without params to be used in COM Objects connections
        /// </summary>
        public ImageSharpQRCode() { }

        public ImageSharpQRCode(QRCodeData data) : base(data) { }

        public Image GetGraphic(int pixelsPerModule)
        {
            return this.GetGraphic(pixelsPerModule, Color.Black, Color.White, true);
        }

        public Image GetGraphic(int pixelsPerModule, string darkColorHtmlHex, string lightColorHtmlHex, bool drawQuietZones = true)
        {
            return this.GetGraphic(pixelsPerModule, Color.Parse(darkColorHtmlHex), Color.Parse(lightColorHtmlHex), drawQuietZones);
        }

        public Image GetGraphic(int pixelsPerModule, Color darkColor, Color lightColor, bool drawQuietZones = true)
        {
            var size = (this.QrCodeData.ModuleMatrix.Count - (drawQuietZones ? 0 : 8)) * pixelsPerModule;
            var offset = drawQuietZones ? 0 : 4 * pixelsPerModule;

            var bmp = new Image<Rgba32>(size, size);
            if (drawQuietZones)
            {
                bmp.Mutate(x => x.Clear(lightColor));
            }
            for (var x = 0; x < size + offset; x += pixelsPerModule)
            {
                for (var y = 0; y < size + offset; y += pixelsPerModule)
                {
                    var module = this.QrCodeData.ModuleMatrix[(y + pixelsPerModule) / pixelsPerModule - 1][(x + pixelsPerModule) / pixelsPerModule - 1];
                    var color = module ? darkColor : lightColor;
                    var rectangle = new Rectangle(x - offset, y - offset, pixelsPerModule, pixelsPerModule);
                    bmp.Mutate(x => x.Fill(color, rectangle));
                }
            }

            return bmp;
        }

        public Image GetGraphic(int pixelsPerModule, Color darkColor, Color lightColor, Image? icon = null, int iconSizePercent = 15, int iconBorderWidth = 0, bool drawQuietZones = true, Color? iconBackgroundColor = null)
        {
            var bmp = GetGraphic(pixelsPerModule, darkColor, lightColor, drawQuietZones);

            DrawingOptions options = new()
            {
                GraphicsOptions = new()
                { 
                    ColorBlendingMode = PixelColorBlendingMode.Normal,
                    Antialias = true
                }
            };

            var drawIconFlag = icon is not null && iconSizePercent > 0 && iconSizePercent <= 100;

            if (drawIconFlag)
            {
                float iconDestWidth = iconSizePercent * bmp.Width / 100f;
                float iconDestHeight = drawIconFlag && icon != null ? iconDestWidth * icon.Height / icon.Width : 0;
                float iconX = (bmp.Width - iconDestWidth) / 2;
                float iconY = (bmp.Height - iconDestHeight) / 2;
                var centerDest = new RectangleF(iconX - iconBorderWidth, iconY - iconBorderWidth, iconDestWidth + iconBorderWidth * 2, iconDestHeight + iconBorderWidth * 2);
                var iconDestRect = new RectangleF(iconX, iconY, iconDestWidth, iconDestHeight);
                var iconBgBrush = iconBackgroundColor ?? lightColor;
                //Only render icon/logo background, if iconBorderWith is set > 0
                if (iconBorderWidth > 0)
                {
                    IPath iconPath = CreateRoundedRectanglePath(centerDest, iconBorderWidth * 2);
                    icon.Mutate(x => x.Resize((int)iconDestWidth, (int)iconDestHeight));
                    IBrush brush = new ImageBrush(icon);
                    bmp.Mutate(x => x.Fill(options, iconBgBrush, iconPath).Fill(options, brush, iconDestRect));
                }
                //gfx.DrawImage(icon, iconDestRect, new RectangleF(0, 0, icon.Width, icon.Height), GraphicsUnit.Pixel);
            }

            return bmp;
        }

        internal static IPath CreateRoundedRectanglePath(RectangleF rect, int cornerRadius)
        {
            var roundedRect = new Polygon(
                    new CubicBezierLineSegment(new PointF(rect.Left, rect.Top + cornerRadius), new PointF(rect.Left, rect.Top + cornerRadius / 2), new PointF(rect.Left + cornerRadius / 2, rect.Top), new PointF(rect.Left + cornerRadius, rect.Top)),
                    new LinearLineSegment(new PointF(rect.Left + cornerRadius, rect.Top), new PointF(rect.Right - cornerRadius, rect.Top)),
                    new CubicBezierLineSegment(new PointF(rect.Right - cornerRadius, rect.Top), new PointF(rect.Right - cornerRadius / 2, rect.Top), new PointF(rect.Right, rect.Top + cornerRadius / 2), new PointF(rect.Right, rect.Top + cornerRadius)),
                    new LinearLineSegment(new PointF(rect.Right, rect.Top + cornerRadius), new PointF(rect.Right, rect.Bottom - cornerRadius)),
                    new CubicBezierLineSegment(new PointF(rect.Right, rect.Bottom - cornerRadius), new PointF(rect.Right, rect.Bottom - cornerRadius / 2), new PointF(rect.Right - cornerRadius / 2, rect.Bottom), new PointF(rect.Right - cornerRadius, rect.Bottom)),
                    new LinearLineSegment(new PointF(rect.Right - cornerRadius, rect.Bottom), new PointF(rect.Left + cornerRadius, rect.Bottom)),
                    new CubicBezierLineSegment(new PointF(rect.Left + cornerRadius, rect.Bottom), new PointF(rect.Left + cornerRadius / 2, rect.Bottom), new PointF(rect.Left, rect.Bottom - cornerRadius / 2), new PointF(rect.Left, rect.Bottom - cornerRadius)),
                    new LinearLineSegment(new PointF(rect.Left, rect.Bottom - cornerRadius), new PointF(rect.Left, rect.Top + cornerRadius))
                );
            return roundedRect.AsClosedPath();
        }
    }

    public static class ImageSharpQRCodeHelper
    {
        public static Image GetQRCode(string plainText, int pixelsPerModule, Color darkColor, Color lightColor, QRCoder.QRCodeGenerator.ECCLevel eccLevel, bool forceUtf8 = false, bool utf8BOM = false, EciMode eciMode = EciMode.Default, int requestedVersion = -1, Image? icon = null, int iconSizePercent = 15, int iconBorderWidth = 0, bool drawQuietZones = true)
        {
            using var qrGenerator = new QRCoder.QRCodeGenerator();
            using var qrCodeData = qrGenerator.CreateQrCode(plainText, eccLevel, forceUtf8, utf8BOM, eciMode, requestedVersion);
            using var qrCode = new ImageSharpQRCode(qrCodeData);
            return qrCode.GetGraphic(pixelsPerModule, darkColor, lightColor, icon, iconSizePercent, iconBorderWidth, drawQuietZones);
        }
    }
}
