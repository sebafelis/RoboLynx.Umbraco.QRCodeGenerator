using RoboLynx.Umbraco.QRCodeGenerator.Models;
using System.Collections.Generic;
using System.Net.Http;
using Umbraco.Core.Composing;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat
{
    public interface IQRCodeFormat : IDiscoverable
    {
        string Id { get; }

        string Name { get; }

        string FileName { get; }

        string Mime { get; }

        IEnumerable<string> RequiredSettings { get; }

        HttpContent ResponseContent(string value, QRCodeSettings settings);

       
    }
}
