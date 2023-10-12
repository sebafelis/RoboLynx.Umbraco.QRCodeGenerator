using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using Umbraco.Core.Composing;
using Umbraco.Core.Configuration;

namespace RoboLynx.Umbraco.QRCodeGenerator
{
    public class QRCodeGeneratorComponent : IComponent
    {
        private readonly IGlobalSettings _globalSettings;

        public QRCodeGeneratorComponent(IGlobalSettings globalSettings)
        {
            _globalSettings = globalSettings;
        }

        public void Initialize()
        {

        }

        public void Terminate()
        {
            // Nothing to terminate
        }
    }
}
