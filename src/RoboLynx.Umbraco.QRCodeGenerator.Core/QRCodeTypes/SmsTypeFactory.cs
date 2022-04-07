using RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources;
using Umbraco.Cms.Core.Services;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes
{
    public class SmsTypeFactory : QRCodeTypeFactory
    {
        public SmsTypeFactory(ILocalizedTextService localizedTextService) : base(localizedTextService)
        {
        }

        public override string Id => "SMS";

        public override IQRCodeType Create(IQRCodeSource qrCodeSource)
        {
            return new SmsType(qrCodeSource);
        }
    }
}