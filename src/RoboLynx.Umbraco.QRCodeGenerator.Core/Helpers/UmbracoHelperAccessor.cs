using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Web;
using Composing = Umbraco.Web.Composing;

namespace RoboLynx.Umbraco.QRCodeGenerator.Helpers
{
    public interface IUmbracoHelperAccessor
    {
        bool TryGetUmbracoHelper(out UmbracoHelper umbracoHelper);
    }

    public class UmbracoHelperAccessor : IUmbracoHelperAccessor
    {

        public bool TryGetUmbracoHelper(out UmbracoHelper umbracoHelper)
        {
            umbracoHelper = Composing.Current.UmbracoHelper;
            return umbracoHelper is not null;
        }
    }
}
