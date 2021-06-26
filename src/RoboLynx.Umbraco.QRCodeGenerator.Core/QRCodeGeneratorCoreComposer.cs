using Chronos.Abstractions;
using DotNetColorParser;
using RoboLynx.Umbraco.QRCodeGenerator.Cache;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Web;

namespace RoboLynx.Umbraco.QRCodeGenerator
{
    public class QRCodeGeneratorCoreComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            composition.RegisterAuto<IQRCodeTypeFactory>();
            composition.RegisterAuto<IQRCodeFormatFactory>();
            composition.RegisterAuto<IQRCodeSourceFactory>();

            composition.Register<IQRCodeCacheManager, QRCodeCacheManager>(Lifetime.Singleton);
            composition.Register<IColorNotationProvider>(f => new ColorNotationProvider(true), Lifetime.Singleton);
            composition.Register<IColorParser, ColorParser>(Lifetime.Singleton);
            composition.Register<IQRCodeBuilder, QRCodeBuilder>(Lifetime.Singleton);
            composition.Register<IQRCodeService, QRCodeService>(Lifetime.Singleton);
        }
    }
}
