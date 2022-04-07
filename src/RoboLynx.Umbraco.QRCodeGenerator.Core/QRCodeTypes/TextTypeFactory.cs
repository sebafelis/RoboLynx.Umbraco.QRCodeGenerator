using RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources;
using Umbraco.Cms.Core.Services;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes
{
    public class TextTypeFactory : QRCodeTypeFactory
    {
        public TextTypeFactory(ILocalizedTextService localizedTextService) : base(localizedTextService)
        {
        }

        public override string Id => "Text";

        public override IQRCodeType Create(IQRCodeSource qrCodeSource)
        {
            return new TextType(qrCodeSource);
        }
    }
}