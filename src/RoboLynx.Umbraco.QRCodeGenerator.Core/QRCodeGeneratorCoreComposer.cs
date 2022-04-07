using Chronos;
using DotNetColorParser;
using Microsoft.Extensions.DependencyInjection;
using RoboLynx.Umbraco.QRCodeGenerator.Cache;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace RoboLynx.Umbraco.QRCodeGenerator
{
    public class QRCodeGeneratorCoreComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            builder.Services.AddDateTimeOffsetProvider();

            builder.QRCodeTypes().Add(() => builder.TypeLoader.GetTypes<IQRCodeTypeFactory>());

            builder.QRCodeSources().Add(() => builder.TypeLoader.GetTypes<IQRCodeSourceFactory>());

            builder.QRCodeFormats().Add(() => builder.TypeLoader.GetTypes<IQRCodeFormatFactory>());

            builder.Services.AddSingleton<IQRCodeHashIdFactory, MD5HashIdFactory>();
            builder.Services.AddSingleton<IQRCodeCacheManager, QRCodeCacheManager>();
            builder.Services.AddSingleton<IColorNotationProvider>(f => new ColorNotationProvider(true));
            builder.Services.AddSingleton<IColorParser, ColorParser>();
            builder.Services.AddSingleton<IQRCodeBuilder, QRCodeBuilder>();
            builder.Services.AddSingleton<IQRCodeService, QRCodeService>();
        }
    }
}