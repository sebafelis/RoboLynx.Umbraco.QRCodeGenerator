using System.Collections.Generic;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources
{
    public class QRCodeSourcesCollection : List<IQRCodeSource>
    {
        public QRCodeSourcesCollection(IEnumerable<IQRCodeSource> items)
            : base(items)
        { }
    }
}
