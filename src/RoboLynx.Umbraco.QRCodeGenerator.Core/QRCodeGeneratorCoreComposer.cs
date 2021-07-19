using Chronos;
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
            composition.Register<IDateTimeOffsetProvider, DateTimeOffsetProvider>();

            composition.QRCodeTypes().Add(() => composition.TypeLoader.GetTypes<IQRCodeTypeFactory>());

            composition.QRCodeSources().Add(() => composition.TypeLoader.GetTypes<IQRCodeSourceFactory>());

            composition.QRCodeFormats().Add(() => composition.TypeLoader.GetTypes<IQRCodeFormatFactory>());

            composition.RegisterUnique<IQRCodeHashIdFactory, MD5HashIdFactory>();
            composition.Register<IQRCodeCacheManager, QRCodeCacheManager>(Lifetime.Singleton);
            composition.Register<IColorNotationProvider>(f => new ColorNotationProvider(true), Lifetime.Singleton);
            composition.Register<IColorParser, ColorParser>(Lifetime.Singleton);
            composition.Register<IQRCodeBuilder, QRCodeBuilder>(Lifetime.Singleton);
            composition.Register<IQRCodeService, QRCodeService>(Lifetime.Singleton);
        }
    }
}
