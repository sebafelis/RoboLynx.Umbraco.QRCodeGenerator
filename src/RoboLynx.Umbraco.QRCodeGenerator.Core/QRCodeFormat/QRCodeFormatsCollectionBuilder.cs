using Umbraco.Core.Composing;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat
{
    public class QRCodeFormatsCollectionBuilder : LazyCollectionBuilderBase<QRCodeFormatsCollectionBuilder, QRCodeFormatsCollection, IQRCodeFormatFactory>
    {
        protected override Lifetime CollectionLifetime => Lifetime.Scope;

        protected override QRCodeFormatsCollectionBuilder This => this;
    }
}
