using RoboLynx.Umbraco.QRCodeGenerator.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.PropertyEditors;

namespace RoboLynx.Umbraco.QRCodeGenerator
{
    public class Starup : ApplicationEventHandler
    {
        private InstallHelpers InstallHelpers => new InstallHelpers();

        protected override void ApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            base.ApplicationStarting(umbracoApplication, applicationContext);
        }

        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            base.ApplicationStarted(umbracoApplication, applicationContext);

            Installation();
        }

        private void CreateDataType(ApplicationContext applicationContext)
        {
            var def = new DataTypeDefinition("robolynx.qrcodegen");
            def.DatabaseType = DataTypeDatabaseType.Nvarchar;
            
            applicationContext.Services.DataTypeService.Save(def);
        }

        private void Installation()
        {
            if (!InstallHelpers.IsInstalled())
            {
                InstallHelpers.Install();
            }
        }
    }
}
