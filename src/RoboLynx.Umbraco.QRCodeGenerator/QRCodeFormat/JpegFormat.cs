using DotNetColorParser;
using RoboLynx.Umbraco.QRCodeGenerator.Controllers;
using RoboLynx.Umbraco.QRCodeGenerator.Models;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Net.Http;
using Umbraco.Core.IO;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat
{
    public class JpegFormat : RasterFormat
    {
        public JpegFormat(ILocalizedTextService localizedTextService, IMediaFileSystem mediaFileSystem, UmbracoHelper umbracoHelper, IColorParser colorParser) : base(localizedTextService, mediaFileSystem, umbracoHelper, colorParser)
        {

        }

        public override string Id => "jpeg";

        public override string FileName => base.FileName + ".jpg";

        public override string Mime => "image/jpeg";

        public override HttpContent ResponseContent(string value, QRCodeSettings settings)
        {
            return RasterResponseContent(value, settings, ImageFormat.Jpeg);
        }
    }
}
