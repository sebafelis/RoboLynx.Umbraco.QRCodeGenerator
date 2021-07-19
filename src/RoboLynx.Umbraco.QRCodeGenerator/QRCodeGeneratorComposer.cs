using DotNetColorParser;
using RoboLynx.Umbraco.QRCodeGenerator.Controllers;
using RoboLynx.Umbraco.QRCodeGenerator.Cache;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Web;
using Chronos.Abstractions;
using Chronos;

namespace RoboLynx.Umbraco.QRCodeGenerator
{
    [ComposeAfter(typeof(QRCodeGeneratorCoreComposer))]
    public class QRCodeGeneratorComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            composition.Register<QRCodeController>(Lifetime.Request);
            composition.Register<QRCodeFormatPickerController>(Lifetime.Request);
            composition.Register<QRCodeLevelPickerController>(Lifetime.Request);
            composition.Register<QRCodeSourcePickerController>(Lifetime.Request);
            composition.Register<QRCodeTypePickerController>(Lifetime.Request);

            composition.ContentApps().Append<QRCodeGeneratorApp>();
        }
    }
}
