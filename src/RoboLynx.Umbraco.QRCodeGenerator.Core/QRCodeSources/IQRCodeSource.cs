using System;
using Umbraco.Core.Composing;
using Umbraco.Core.Models.PublishedContent;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources
{
    public interface IQRCodeSource : IDiscoverable
    {
        string Id { get; }
        string Name { get; }
        string Description { get; }
        T GetValue<T>(int index, string key, IPublishedContent content, string sourceSettings, string culture) where T : IConvertible;
    }
}
