using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources
{
    public interface IQRCodeSourceFactory : IDiscoverable
    {
        string Id { get; }
        string Name { get; }
        string Description { get; }

        IQRCodeSource Create(IPublishedContent publishedContent, string sourceSettings, string culture);
    }
}