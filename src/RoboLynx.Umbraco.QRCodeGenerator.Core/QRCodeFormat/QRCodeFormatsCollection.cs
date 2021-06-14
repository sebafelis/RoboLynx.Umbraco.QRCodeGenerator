using System.Collections.Generic;
using Umbraco.Core.Composing;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat
{
    public class QRCodeFormatsCollection : BuilderCollectionBase<IQRCodeFormatFactory>
    {
        public QRCodeFormatsCollection(IEnumerable<IQRCodeFormatFactory> items)
            : base(items)
        { }
    }
}
