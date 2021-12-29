using Chronos;
using Chronos.Abstractions;
using DotNetColorParser;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes;
using Umbraco.Core;
using Umbraco.Core.Composing;

namespace RoboLynx.Umbraco.QRCodeGenerator.Cache
{
    public class QRCodeGeneratorCacheComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            composition.Register<IQRCodeCacheFactory, QRCodeCacheFactory>(Lifetime.Singleton);
            composition.Register<IQRCodeCacheFileSystemFactory, QRCodeCacheFileSystemFactory>(Lifetime.Singleton);
        }
    }
}
