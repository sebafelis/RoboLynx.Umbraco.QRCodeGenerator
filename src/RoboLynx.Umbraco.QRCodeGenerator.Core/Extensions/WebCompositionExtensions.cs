using RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes;
using Umbraco.Cms.Core.DependencyInjection;

namespace RoboLynx.Umbraco.QRCodeGenerator.Extensions
{
    public static class WebCompositionExtensions
    {
        public static QRCodeTypeFactoryCollectionBuilder QRCodeTypes(this IUmbracoBuilder composition)
            => composition.WithCollectionBuilder<QRCodeTypeFactoryCollectionBuilder>();

        public static QRCodeSourceFactoryCollectionBuilder QRCodeSources(this IUmbracoBuilder composition)
            => composition.WithCollectionBuilder<QRCodeSourceFactoryCollectionBuilder>();

        public static QRCodeFormatFactoryCollectionBuilder QRCodeFormats(this IUmbracoBuilder composition)
            => composition.WithCollectionBuilder<QRCodeFormatFactoryCollectionBuilder>();
    }
}