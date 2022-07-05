using System;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources
{
    public abstract class QRCodeSource : IQRCodeSource
    {
        public QRCodeSource()
        {
        }

        public abstract T? GetValue<T>(int index, string key) where T : IConvertible;
    }
}