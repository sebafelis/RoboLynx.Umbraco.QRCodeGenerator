using System;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources
{
    public interface IQRCodeSource
    {
        string Id { get; }
        string Name { get; }
        string Description { get; }
        T GetValue<T>(int index, string key, IPublishedContent content, string sourceSettings, string culture) where T : IConvertible;
    }
}
