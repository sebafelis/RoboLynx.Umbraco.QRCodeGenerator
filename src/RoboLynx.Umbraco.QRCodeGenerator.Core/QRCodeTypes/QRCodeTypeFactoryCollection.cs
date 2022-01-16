using System;
using System.Collections.Generic;
using Umbraco.Cms.Core.Composing;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes
{
    public class QRCodeTypeFactoryCollection : BuilderCollectionBase<IQRCodeTypeFactory>
    {
        public QRCodeTypeFactoryCollection(Func<IEnumerable<IQRCodeTypeFactory>> items)
            : base(items)
        { }
    }
}
