using Umbraco.Core.Composing;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources
{
    public class QRCodeSourcesCollectionBuilder : LazyCollectionBuilderBase<QRCodeSourcesCollectionBuilder, QRCodeSourcesCollection, IQRCodeSource>
    {
        protected override Lifetime CollectionLifetime => Lifetime.Scope;

        protected override QRCodeSourcesCollectionBuilder This => this;
    }
}
