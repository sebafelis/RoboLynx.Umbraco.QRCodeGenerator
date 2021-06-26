using System;
using Umbraco.Core.Composing;
using Umbraco.Core.Models.PublishedContent;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources
{
    public interface IQRCodeSource : IDiscoverable
    {
        T GetValue<T>(int index, string key) where T : IConvertible;
    }
}
