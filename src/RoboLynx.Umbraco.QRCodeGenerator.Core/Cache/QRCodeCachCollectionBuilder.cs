using RoboLynx.Umbraco.QRCodeGenerator.Cache;
using Umbraco.Core.Composing;

namespace RoboLynx.Umbraco.QRCodeGenerator.Cache
{
    public class QRCodeCachCollectionBuilder : CollectionBuilderBase<QRCodeCachCollectionBuilder, QRCodeCachCollection, IQRCodeCache>
    {
        protected override Lifetime CollectionLifetime => Lifetime.Singleton;
    }
}
