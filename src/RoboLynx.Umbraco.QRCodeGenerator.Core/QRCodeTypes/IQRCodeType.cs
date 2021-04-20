using RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources;
using Umbraco.Core.Models;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes
{
    public interface IQRCodeType
    {
        string Id { get; }
        string Name { get; }
        string Description { get; }
        string Value(IQRCodeSource source, string sourceSettings, IPublishedContent content, string culture);
    }
}
