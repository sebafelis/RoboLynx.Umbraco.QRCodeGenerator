using System;
using Umbraco.Cms.Core.Composing;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources
{
    public interface IQRCodeSource : IDiscoverable
    {
        T? GetValue<T>(int index, string key) where T : IConvertible;
    }
}