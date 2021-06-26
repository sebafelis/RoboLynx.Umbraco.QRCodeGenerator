using System.Collections.Generic;
using Umbraco.Core.Composing;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources
{
    public class QRCodeSourceFactoryCollection : BuilderCollectionBase<IQRCodeSourceFactory>
    {
        public QRCodeSourceFactoryCollection(IEnumerable<IQRCodeSourceFactory> items)
            : base(items)
        { }
    }
}
