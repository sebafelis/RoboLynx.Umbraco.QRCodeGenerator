using System;
using System.Collections.Generic;
using Umbraco.Cms.Core.Composing;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources
{
    public class QRCodeSourceFactoryCollection : BuilderCollectionBase<IQRCodeSourceFactory>
    {
        public QRCodeSourceFactoryCollection(Func<IEnumerable<IQRCodeSourceFactory>> items)
            : base(items)
        { }
    }
}
