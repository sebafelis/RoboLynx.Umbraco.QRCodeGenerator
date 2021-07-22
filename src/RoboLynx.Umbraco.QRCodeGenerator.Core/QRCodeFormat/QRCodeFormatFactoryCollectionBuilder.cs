using Umbraco.Core.Composing;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat
{
    public class QRCodeFormatFactoryCollectionBuilder : LazyCollectionBuilderBase<QRCodeFormatFactoryCollectionBuilder, QRCodeFormatFactoryCollection, IQRCodeFormatFactory>
    {
        public QRCodeFormatFactoryCollectionBuilder()
        {
            
        }

        protected override Lifetime CollectionLifetime => Lifetime.Scope;

        protected override QRCodeFormatFactoryCollectionBuilder This => this;

        //public override QRCodeFormatFactoryCollection CreateCollection(IFactory factory)
        //{
        //    var collection = base.CreateCollection(factory);

        //    collection. factory.GetAllInstances<IQRCodeFormat>();

        //}
    }
}
