using RoboLynx.Umbraco.QRCodeGenerator.Models;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using Umbraco.Core.Composing;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat
{
    public interface IQRCodeFormat : IDiscoverable
    {
        string FileName { get; }

        string Mime { get; }

        string FileExtension { get; }

        Stream Stream();
    }
}
