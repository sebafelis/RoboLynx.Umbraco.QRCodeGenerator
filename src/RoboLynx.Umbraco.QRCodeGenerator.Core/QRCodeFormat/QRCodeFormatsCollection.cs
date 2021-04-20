using System.Collections.Generic;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat
{
    public class QRCodeFormatsCollection : List<IQRCodeFormat>
    {
        public QRCodeFormatsCollection(IEnumerable<IQRCodeFormat> items)
           : base(items)
        { }
    }
}
