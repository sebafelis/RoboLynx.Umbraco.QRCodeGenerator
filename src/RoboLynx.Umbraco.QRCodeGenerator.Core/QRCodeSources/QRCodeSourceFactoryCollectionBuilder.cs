using Umbraco.Core.Composing;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources
{
    public class QRCodeSourceFactoryCollectionBuilder : LazyCollectionBuilderBase<QRCodeSourceFactoryCollectionBuilder, QRCodeSourceFactoryCollection, IQRCodeSourceFactory>
    {
        protected override Lifetime CollectionLifetime => Lifetime.Scope;

        protected override QRCodeSourceFactoryCollectionBuilder This => this;
    }
}
