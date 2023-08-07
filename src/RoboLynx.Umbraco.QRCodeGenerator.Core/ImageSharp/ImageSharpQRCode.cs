using QRCoder;
using System;
using SixLabors.ImageSharp;
using static QRCoder.QRCodeGenerator;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Advanced;
using System.Collections.Generic;

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


            bmp.DrawRectangle(0, 0, size, size, lightColor);

            for (var x = 0; x < size + offset; x += pixelsPerModule)
            {
                for (var y = 0; y < size + offset; y += pixelsPerModule)
                {
                    var module = this.QrCodeData.ModuleMatrix[(y + pixelsPerModule) / pixelsPerModule - 1][(x + pixelsPerModule) / pixelsPerModule - 1];
                    if (module)
                    {
                        bmp.DrawRectangle(x - offset, y - offset, pixelsPerModule, pixelsPerModule, darkColor);
                    }
                }
            }
            return bmp;
        }



        public Image GetGraphic(int pixelsPerModule, Color darkColor, Color lightColor, Image? icon = null, int iconSizePercent = 15, int iconBorderWidth = 0, bool drawQuietZones = true, Color? iconBackgroundColor = null)
        {
            var bmp = GetGraphic(pixelsPerModule, darkColor, lightColor, drawQuietZones);

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

                icon.Mutate(x => x.Resize((int)iconDestWidth, (int)iconDestHeight));

                var iconRgba = icon.CloneAs<Rgba32>();

                if (iconBorderWidth > 0)
                {
                    var iconBorder = CreateIconBorder(iconRgba, iconBorderWidth, iconBackgroundColor ?? lightColor);

                    bmp.Mutate(x => x.DrawImage(iconBorder, new Point((int)iconX - iconBorderWidth, (int)iconY - iconBorderWidth), 1));
                }

                bmp.Mutate(x => x.DrawImage(iconRgba, new Point((int)iconX, (int)iconY), 1));
            }

            return bmp;
        }


        private Image CreateIconBorder(Image<Rgba32> icon, int iconBorderWidth, Color iconBackgroundColor)
        {
            //TODO: Use Span to make method more efficient (https://docs.sixlabors.com/articles/imagesharp/pixelbuffers.html)

            var iconBorder = new Image<Rgba32>(icon.Width + iconBorderWidth * 2, icon.Height + iconBorderWidth * 2);

            // Define the directions for 8-connectivity (top, bottom, left, right)
            var directions = new List<(int dx, int dy)>
                    {
                        (0, -1), // Top
                        (-1, -1), // Top-left
                        (1, -1), // Top-Right
                        (0, 1),  // Bottom
                        (-1, 1),  // Bottom-left
                        (1, 1),  // Bottom-right
                        (-1, 0), // Left
                        (1, 0)   // Right
                    };

            // Iterate through each pixel to find the non-transparent region's contour
            for (int y = 0; y < icon.Height; y++)
            {
                for (int x = 0; x < icon.Width; x++)
                {
                    if (icon[x, y].A == 0) // Transparent pixel, skip
                        continue;

                    // Check the neighboring pixels for transparent pixels
                    foreach (var (dx, dy) in directions)
                    {
                        int nx = x + dx;
                        int ny = y + dy;

                        // Check if the neighboring pixel is outside the image boundary or transparent
                        //
                        if (nx < 0 || nx >= icon.Width || ny < 0 || ny >= icon.Height || icon[nx, ny].A == 0)
                        {
                            // If a transparent pixel is found, it means we are on the boundary, draw the border
                            for (int i = 1; i <= iconBorderWidth; i++)
                            {
                                int bx = x + iconBorderWidth + i * dx;
                                int by = y + iconBorderWidth + i * dy;

                                if (bx >= 0 && bx < iconBorder.Width && by >= 0 && by < iconBorder.Height)
                                {
                                    iconBorder[bx, by] = iconBackgroundColor;
                                }
                            }
                        }
                    }
                }
            }
            return iconBorder;
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
