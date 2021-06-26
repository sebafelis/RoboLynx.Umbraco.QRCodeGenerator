using Umbraco.Core.Composing;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes
{
    public class QRCodeTypeFactoryCollectionBuilder : LazyCollectionBuilderBase<QRCodeTypeFactoryCollectionBuilder, QRCodeTypeFactoryCollection, IQRCodeTypeFactory>
    {
        protected override Lifetime CollectionLifetime => Lifetime.Scope;

        protected override QRCodeTypeFactoryCollectionBuilder This => this;
    }
}
