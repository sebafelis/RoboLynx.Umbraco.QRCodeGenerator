using RoboLynx.Umbraco.QRCodeGenerator.Frontend.Controllers;
using RoboLynx.Umbraco.QRCodeGenerator.Helpers;
using System;
using Umbraco.Core;
using Umbraco.Core.Composing;

namespace RoboLynx.Umbraco.QRCodeGenerator.Frontend
{
    [ComposeAfter(typeof(QRCodeGeneratorComposer))]
    public class QRCodeGeneratorFrontendComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            var config = CreateConfiguration();

            if (!config.Disable)
            {
                composition.Register<PublicQRCodeController>(Lifetime.Request);
            }
        }

        private QRCodeFrontendConfig CreateConfiguration()
        {

            var disable = ConfigurationHelper.GetAppSetting(Constants.Configuration.DisableKey) != null
                            && ConfigurationHelper.GetAppSetting(Constants.Configuration.DisableKey)
                            .Equals("true", StringComparison.InvariantCultureIgnoreCase);


            return new QRCodeFrontendConfig
            {
                Disable = disable
            };
        }
    }
}
