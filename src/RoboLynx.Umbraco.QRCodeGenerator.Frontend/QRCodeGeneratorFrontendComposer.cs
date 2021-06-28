using Chronos.Abstractions;
using DotNetColorParser;
using RoboLynx.Umbraco.QRCodeGenerator.Cache;
using RoboLynx.Umbraco.QRCodeGenerator.Frontend.Controllers;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes;
using System.Web.Http;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Web;

namespace RoboLynx.Umbraco.QRCodeGenerator.Frontend
{
    public class QRCodeGeneratorFrontendComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            var routers = GlobalConfiguration.Configuration.Routes;

            composition.Register<QRCodePublicController>(Lifetime.Singleton);
        }
    }
}
