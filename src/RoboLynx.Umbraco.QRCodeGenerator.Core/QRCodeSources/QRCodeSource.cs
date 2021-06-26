using System;
using Umbraco.Core;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.Services;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources
{
    public abstract class QRCodeSource : IQRCodeSource
    {
        public QRCodeSource()
        {

        }

        public abstract T GetValue<T>(int index, string key) where T : IConvertible;
    }
}
