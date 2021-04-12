using RoboLynx.Umbraco.QRCodeGenerator.Controllers;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Net.Http;
using Umbraco.Web;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat
{
    public class BmpFormat : RasterFormat 
    {
        public override string Id => "bmp";

        public override string FileName => base.FileName + ".bmp";

        public override HttpContent ResponseContent(string value, QRCodeSettings settings, UmbracoHelper umbracoHelper)
        {
            return RasterResponseContent(value, settings, umbracoHelper, ImageFormat.Bmp, "image/bmp");
        }
    }
}
