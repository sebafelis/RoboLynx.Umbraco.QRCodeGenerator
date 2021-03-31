using RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes;
using Umbraco.Core.Composing;

namespace RoboLynx.Umbraco.QRCodeGenerator
{
    public static class WebCompositionExtensions
    {
        public static QRCodeTypesCollectionBuilder QRCodeTypes(this Composition composition)
            => composition.WithCollectionBuilder<QRCodeTypesCollectionBuilder>();

        public static QRCodeSourcesCollectionBuilder QRCodeSources(this Composition composition)
            => composition.WithCollectionBuilder<QRCodeSourcesCollectionBuilder>();

        public static QRCodeFormatsCollectionBuilder QRCodeFormats(this Composition composition)
            => composition.WithCollectionBuilder<QRCodeFormatsCollectionBuilder>();
    }
}
