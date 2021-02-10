using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Composing;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes
{
    public class QRCodeTypesCollectionBuilder : LazyCollectionBuilderBase<QRCodeTypesCollectionBuilder, QRCodeTypesCollection, IQRCodeType>
    {
        protected override Lifetime CollectionLifetime => Lifetime.Scope;

        protected override QRCodeTypesCollectionBuilder This => this;
    }
}
