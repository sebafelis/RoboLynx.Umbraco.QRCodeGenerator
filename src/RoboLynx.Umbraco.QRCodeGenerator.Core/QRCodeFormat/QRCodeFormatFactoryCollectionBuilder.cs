using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.Composing;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat
{
    public class QRCodeFormatFactoryCollectionBuilder : LazyCollectionBuilderBase<QRCodeFormatFactoryCollectionBuilder, QRCodeFormatFactoryCollection, IQRCodeFormatFactory>
    {

        protected override ServiceLifetime CollectionLifetime => ServiceLifetime.Singleton;

        protected override QRCodeFormatFactoryCollectionBuilder This => this;
    }
}
