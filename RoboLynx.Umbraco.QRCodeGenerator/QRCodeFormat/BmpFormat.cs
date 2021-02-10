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
    public class BmpFormat : RasterFormat 
    {
        public BmpFormat(ILocalizedTextService localizedTextService, IMediaFileSystem mediaFileSystem, UmbracoHelper umbracoHelper) : base(localizedTextService, mediaFileSystem, umbracoHelper)
        {

        }

        public override string Id => "bmp";

        public override string FileName => base.FileName + ".bmp";

        public override string Mime => "image/bmp";

        public override HttpContent ResponseContent(string value, QRCodeSettings settings)
        {
            return RasterResponseContent(value, settings, ImageFormat.Bmp);
        }
    }
}
