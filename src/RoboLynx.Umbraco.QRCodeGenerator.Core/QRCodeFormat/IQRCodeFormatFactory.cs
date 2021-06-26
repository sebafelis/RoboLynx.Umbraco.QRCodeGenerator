using RoboLynx.Umbraco.QRCodeGenerator.Models;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes;
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

        IQRCodeFormat Create(IQRCodeType codeType, QRCodeSettings settings);
    }
}
