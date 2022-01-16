using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.Composing;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes
{
    public class QRCodeTypeFactoryCollectionBuilder : LazyCollectionBuilderBase<QRCodeTypeFactoryCollectionBuilder, QRCodeTypeFactoryCollection, IQRCodeTypeFactory>
    {
        protected override ServiceLifetime CollectionLifetime => ServiceLifetime.Singleton;

        protected override QRCodeTypeFactoryCollectionBuilder This => this;
    }
}
