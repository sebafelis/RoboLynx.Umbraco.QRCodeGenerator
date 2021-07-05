using RoboLynx.Umbraco.QRCodeGenerator.Cache;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Umbraco.Core.Composing;

namespace RoboLynx.Umbraco.QRCodeGenerator.Cache
{
    public class QRCodeCachCollection : BuilderCollectionBase<IQRCodeCache>
    {
        public QRCodeCachCollection(IEnumerable<IQRCodeCache> items)
            : base(items)
        {
            if (items.GroupBy(t => t.Name).Where(x => x.Count() > 1).Any())
            {
                throw new DuplicateNameException("More then one cache with the same name for QR Code Generator was register.");
            }
        }
    }
}
