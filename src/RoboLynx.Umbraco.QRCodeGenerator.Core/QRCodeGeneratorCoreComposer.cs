using Chronos;
using DotNetColorParser;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RoboLynx.Umbraco.QRCodeGenerator.Cache;
using RoboLynx.Umbraco.QRCodeGenerator.Exceptions;
using RoboLynx.Umbraco.QRCodeGenerator.Extensions;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes;
using System.Linq;
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

            builder.QRCodeFormats().Add(() =>
            {
                var formatTypes = builder.TypeLoader.GetTypes<IQRCodeFormatFactory>();
                if (!formatTypes.Any())
                {
                    throw new QRCodeGeneratorException("Not found any class implementing IQRCodeFormat interface. Add RoboLynx.Umbraco.QRCodeGenerator.Core.ImageSharp or RoboLynx.Umbraco.QRCodeGenerator.Core.ImageSharp2 package to project.");
                }

                return formatTypes;
            });


            if (!builder.TypeLoader.GetTypes<IQRCodeFormatFactory>().Any()) { }

            builder.Services.AddSingleton<IQRCodeHashIdFactory, MD5HashIdFactory>();
            builder.Services.AddSingleton<IQRCodeCacheManager, QRCodeCacheManager>();
            builder.Services.AddSingleton<IColorNotationProvider>(f => new ColorNotationProvider(true));
            builder.Services.AddSingleton<IColorParser, ColorParser>();
            builder.Services.AddSingleton<IQRCodeBuilder, QRCodeBuilder>();
            builder.Services.AddSingleton<IQRCodeService, QRCodeService>();
        }
    }
}