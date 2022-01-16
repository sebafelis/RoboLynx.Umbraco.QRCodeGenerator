using System.IO;
using Umbraco.Cms.Core.Composing;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat
{
    public interface IQRCodeFormat : IDiscoverable
    {
        string FileName { get; }

        string Mime { get; }

        string FileExtension { get; }

        string HashId { get; }

        Stream Stream();
    }
}
