using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Composing;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources
{
    public class QRCodeSourcesCollection : BuilderCollectionBase<IQRCodeSource>
    {
        public QRCodeSourcesCollection(IEnumerable<IQRCodeSource> items)
            : base(items)
        { }
    }
}
