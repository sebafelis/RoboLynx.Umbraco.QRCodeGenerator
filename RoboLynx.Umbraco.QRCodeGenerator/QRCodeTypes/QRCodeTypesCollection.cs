using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
