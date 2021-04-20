using System.Collections.Generic;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes
{
    public class QRCodeTypesCollection : List<IQRCodeType>
    {
        public QRCodeTypesCollection(IEnumerable<IQRCodeType> items)
            : base(items)
        { }
    }
}
