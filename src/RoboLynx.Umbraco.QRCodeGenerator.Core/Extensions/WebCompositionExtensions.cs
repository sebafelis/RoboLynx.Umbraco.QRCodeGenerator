using RoboLynx.Umbraco.QRCodeGenerator.Cache;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes;
using Umbraco.Core.Composing;

namespace RoboLynx.Umbraco.QRCodeGenerator
{
    public static class WebCompositionExtensions
    {
        public static QRCodeTypeFactoryCollectionBuilder QRCodeTypes(this Composition composition)
            => composition.WithCollectionBuilder<QRCodeTypeFactoryCollectionBuilder>();

        public static QRCodeSourceFactoryCollectionBuilder QRCodeSources(this Composition composition)
            => composition.WithCollectionBuilder<QRCodeSourceFactoryCollectionBuilder>();

        public static QRCodeFormatFactoryCollectionBuilder QRCodeFormats(this Composition composition)
            => composition.WithCollectionBuilder<QRCodeFormatFactoryCollectionBuilder>();

        public static QRCodeCachCollectionBuilder QRCodeCaches(this Composition composition)
            => composition.WithCollectionBuilder<QRCodeCachCollectionBuilder>();
    }
}
