using RoboLynx.Umbraco.QRCodeGenerator.Controllers;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Web;

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
