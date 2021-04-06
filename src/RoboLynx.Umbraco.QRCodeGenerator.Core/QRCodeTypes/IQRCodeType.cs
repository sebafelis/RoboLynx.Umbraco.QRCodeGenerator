using RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources;
using Umbraco.Core.Composing;
using Umbraco.Core.Models.PublishedContent;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes
{
    public interface IQRCodeType : IDiscoverable
    {
        string Id { get; }
        string Name { get; }
        string Description { get; }
        string Value(IQRCodeSource source, string sourceSettings, IPublishedContent content, string culture);
    }
}
