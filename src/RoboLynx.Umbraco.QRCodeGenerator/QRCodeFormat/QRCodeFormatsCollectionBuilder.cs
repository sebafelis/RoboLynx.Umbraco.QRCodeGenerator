using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Composing;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat
{
    public class QRCodeFormatsCollectionBuilder : LazyCollectionBuilderBase<QRCodeFormatsCollectionBuilder, QRCodeFormatsCollection, IQRCodeFormat>
    {
        protected override Lifetime CollectionLifetime => Lifetime.Scope;

        protected override QRCodeFormatsCollectionBuilder This => this;
    }
}
