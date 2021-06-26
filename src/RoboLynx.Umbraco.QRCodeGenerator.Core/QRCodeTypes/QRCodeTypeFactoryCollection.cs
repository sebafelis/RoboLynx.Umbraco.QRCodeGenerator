using System.Collections.Generic;
using Umbraco.Core.Composing;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes
{
    public class QRCodeTypeFactoryCollection : BuilderCollectionBase<IQRCodeTypeFactory>
    {
        public QRCodeTypeFactoryCollection(IEnumerable<IQRCodeTypeFactory> items)
            : base(items)
        { }
    }
}
