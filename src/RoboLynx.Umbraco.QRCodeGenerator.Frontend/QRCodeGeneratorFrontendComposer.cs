using RoboLynx.Umbraco.QRCodeGenerator.Frontend.Controllers;
using Umbraco.Core;
using Umbraco.Core.Composing;

namespace RoboLynx.Umbraco.QRCodeGenerator.Frontend
{
    [ComposeAfter(typeof(QRCodeGeneratorComposer))]
    public class QRCodeGeneratorFrontendComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            composition.Register<PublicQRCodeController>(Lifetime.Request);
        }
    }
}
