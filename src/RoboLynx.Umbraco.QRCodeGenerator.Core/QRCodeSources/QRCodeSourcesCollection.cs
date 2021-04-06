using System.Collections.Generic;
using Umbraco.Core.Composing;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources
{
    public class QRCodeSourcesCollection : BuilderCollectionBase<IQRCodeSource>
    {
        public QRCodeSourcesCollection(IEnumerable<IQRCodeSource> items)
            : base(items)
        { }
    }
}
