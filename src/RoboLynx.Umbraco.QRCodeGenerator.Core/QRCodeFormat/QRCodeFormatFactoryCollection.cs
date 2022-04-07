using System;
using System.Collections.Generic;
using Umbraco.Cms.Core.Composing;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat
{
    public class QRCodeFormatFactoryCollection : BuilderCollectionBase<IQRCodeFormatFactory>
    {
        public QRCodeFormatFactoryCollection(Func<IEnumerable<IQRCodeFormatFactory>> items)
            : base(items)
        { }
    }
}