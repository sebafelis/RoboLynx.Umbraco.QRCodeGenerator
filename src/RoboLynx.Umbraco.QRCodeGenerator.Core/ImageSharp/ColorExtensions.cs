using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboLynx.Umbraco.QRCodeGenerator.ImageSharp
{
    internal static class ColorExtensions
    {
        public static System.Drawing.Color ToSystemDrawingColor(this SixLabors.ImageSharp.Color c)
        {
            var converted = c.ToPixel<Argb32>();
            return System.Drawing.Color.FromArgb((int)converted.Argb);
        }
        public static SixLabors.ImageSharp.Color ToImageSharpColor(this System.Drawing.Color c)
        {
            return SixLabors.ImageSharp.Color.FromRgba(c.R, c.G, c.B, c.A);
        }
    }
}
