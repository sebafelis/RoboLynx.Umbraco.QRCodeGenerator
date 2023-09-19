using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using System;

namespace RoboLynx.Umbraco.QRCodeGenerator.ImageSharp
{
    internal static class DrawShapeExtensions
    {
        public static Image<TPixel> DrawRectangle<TPixel>(this Image<TPixel> image, int offsetX, int offsetY, int width, int height, Color color) where TPixel : unmanaged, IPixel<TPixel>
        {
            int totalWidth = image.Width;
            int totalHeight = image.Height;

            var startX = offsetX;
            var startY = offsetY;
            var endX = startX + width;
            var endY = startY + height;

            if (startX < 0 && startY < 0 && endX > totalWidth &&  endY > totalHeight)
            {
                throw new ArgumentException("Rectangle coordinates out of image area.");
            }

            for (int x = startX; x < endX; x++)
            {
                for (var y = startY; y < endY; y++)
                {
                    var pixel = new TPixel();
                    pixel.FromRgba32(color);
                    image[x, y] = pixel;
                }
            }
            return image;
        }
    }
}
