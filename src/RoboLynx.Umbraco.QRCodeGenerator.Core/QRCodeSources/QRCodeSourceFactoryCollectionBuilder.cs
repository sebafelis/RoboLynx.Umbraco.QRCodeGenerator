using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.Composing;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources
{
    public class QRCodeSourceFactoryCollectionBuilder : LazyCollectionBuilderBase<QRCodeSourceFactoryCollectionBuilder, QRCodeSourceFactoryCollection, IQRCodeSourceFactory>
    {
        protected override ServiceLifetime CollectionLifetime => ServiceLifetime.Singleton;

        protected override QRCodeSourceFactoryCollectionBuilder This => this;
    }
}
