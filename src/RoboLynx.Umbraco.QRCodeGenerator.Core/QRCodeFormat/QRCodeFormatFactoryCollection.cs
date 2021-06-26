using System.Collections.Generic;
using Umbraco.Core.Composing;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat
{
    public class QRCodeFormatFactoryCollection : BuilderCollectionBase<IQRCodeFormatFactory>
    {
        public QRCodeFormatFactoryCollection(IEnumerable<IQRCodeFormatFactory> items)
            : base(items)
        { }
    }
}
