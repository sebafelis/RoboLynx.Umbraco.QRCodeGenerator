using DotNetColorParser;
using RoboLynx.Umbraco.QRCodeGenerator.Models;
using System.Drawing.Imaging;
using System.Net.Http;
using Umbraco.Core.IO;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat
{
    public class PngFormat : RasterFormat
    {
        public PngFormat(ILocalizedTextService localizedTextService, IMediaService mediaFileSystem, UmbracoHelper umbracoHelper, IColorParser colorParser) : base(localizedTextService, mediaFileSystem, umbracoHelper, colorParser)
        {

        }

        public override string Id => "png";

        public override string FileName => base.FileName + ".png";

        public override string Mime => "image/png";

        public override HttpContent ResponseContent(string value, QRCodeSettings settings)
        {
            return RasterResponseContent(value, settings, ImageFormat.Png);
        }
    }
}
