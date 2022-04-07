using RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources;
using Umbraco.Cms.Core.Services;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes
{
    public class UrlTypeFactory : QRCodeTypeFactory
    {
        public UrlTypeFactory(ILocalizedTextService localizedTextService) : base(localizedTextService)
        {
        }

        public override string Id => "URL";

        public override IQRCodeType Create(IQRCodeSource qrCodeSource)
        {
            return new UrlType(qrCodeSource);
        }
    }
}