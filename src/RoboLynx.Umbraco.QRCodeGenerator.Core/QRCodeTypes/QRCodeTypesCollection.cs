using System.Collections.Generic;
using Umbraco.Core.Composing;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes
{
    public class QRCodeTypesCollection : BuilderCollectionBase<IQRCodeType>
    {
        public QRCodeTypesCollection(IEnumerable<IQRCodeType> items)
            : base(items)
        { }
    }
}
