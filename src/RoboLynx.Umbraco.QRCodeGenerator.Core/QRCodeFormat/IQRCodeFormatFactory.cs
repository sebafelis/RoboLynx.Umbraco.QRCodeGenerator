using RoboLynx.Umbraco.QRCodeGenerator.Models;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using Umbraco.Core.Composing;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat
{
    public interface IQRCodeFormatFactory : IDiscoverable
    {
        string Id { get; }

        string Name { get; }

        IEnumerable<string> RequiredSettings { get; }

        IQRCodeFormat Create(string codeContent, QRCodeSettings settings);
    }
}
