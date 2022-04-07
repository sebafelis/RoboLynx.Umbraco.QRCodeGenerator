using RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources;
using Umbraco.Cms.Core.Services;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes
{
    public class PhoneNumberTypeFactory : QRCodeTypeFactory
    {
        public PhoneNumberTypeFactory(ILocalizedTextService localizedTextService) : base(localizedTextService)
        {
        }

        public override string Id => "PhoneNumber";

        public override IQRCodeType Create(IQRCodeSource qrCodeSource)
        {
            return new PhoneNumberType(qrCodeSource);
        }
    }
}