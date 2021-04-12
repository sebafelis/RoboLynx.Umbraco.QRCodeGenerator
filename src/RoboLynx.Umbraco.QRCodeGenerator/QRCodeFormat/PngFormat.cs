using RoboLynx.Umbraco.QRCodeGenerator.Controllers;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Net.Http;
using Umbraco.Web;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat
{
    public class PngFormat : RasterFormat
    {
        public override string Id => "png";

        public override string FileName => base.FileName + ".png";

        public override HttpContent ResponseContent(string value, QRCodeSettings settings, UmbracoHelper umbracoHelper)
        {
            return RasterResponseContent(value, settings, umbracoHelper, ImageFormat.Png, "image/png");
        }
    }
}
