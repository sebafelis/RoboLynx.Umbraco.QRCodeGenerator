using System.Collections.Generic;
using Umbraco.Core.Composing;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat
{
    public class QRCodeFormatsCollection : BuilderCollectionBase<IQRCodeFormat>
    {
        public QRCodeFormatsCollection(IEnumerable<IQRCodeFormat> items)
            : base(items)
        { }
    }
}
