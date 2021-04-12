using RoboLynx.Umbraco.QRCodeGenerator.Controllers;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Net.Http;
using Umbraco.Web;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat
{
    public class JpegFormat : RasterFormat
    {
        public override string Id => "jpeg";

        public override string FileName => base.FileName + ".jpg"; 

        public override HttpContent ResponseContent(string value, QRCodeSettings settings, UmbracoHelper umbracoHelper)
        {
            return RasterResponseContent(value, settings, umbracoHelper, ImageFormat.Jpeg, "image/jpeg");
        }
    }
}
