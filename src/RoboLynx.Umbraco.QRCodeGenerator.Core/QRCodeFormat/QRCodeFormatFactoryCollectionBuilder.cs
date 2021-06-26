using Umbraco.Core.Composing;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat
{
    public class QRCodeFormatFactoryCollectionBuilder : LazyCollectionBuilderBase<QRCodeFormatFactoryCollectionBuilder, QRCodeFormatFactoryCollection, IQRCodeFormatFactory>
    {
        protected override Lifetime CollectionLifetime => Lifetime.Scope;

        protected override QRCodeFormatFactoryCollectionBuilder This => this;
    }
}
