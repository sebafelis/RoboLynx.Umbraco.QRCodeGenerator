using RoboLynx.Umbraco.QRCodeGenerator.Controllers;
using System.Collections.Generic;
using System.Net.Http;
using Umbraco.Web;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat
{
    public interface IQRCodeFormat
    {
        string Id { get; }

        string Name { get; }

        string FileName { get; }

        IEnumerable<string> RequiredSettings { get; }

        HttpContent ResponseContent(string value, QRCodeSettings settings, UmbracoHelper umbracoHelper);

       
    }
}
