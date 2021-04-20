using LightInject;
using Our.Umbraco.IoC.LightInject;
using RoboLynx.Umbraco.QRCodeGenerator.Controllers;
using RoboLynx.Umbraco.QRCodeGenerator.Core;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;

namespace RoboLynx.Umbraco.QRCodeGenerator
{
    public class Startup : ApplicationEventHandler
    {
        protected override void ApplicationInitialized(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {           
            LightInjectStartup.ContainerBuilding += (sender, args) =>
            {
                args.Container.RegisterFrom<Compositor>();

                //add our own services
                args.Container.RegisterApiControllers(typeof(QRCodeController).Assembly);
            };
        }
    }
}
