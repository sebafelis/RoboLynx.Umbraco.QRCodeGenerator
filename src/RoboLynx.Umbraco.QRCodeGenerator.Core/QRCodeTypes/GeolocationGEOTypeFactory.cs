using RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources;
using Umbraco.Core.Services;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes
{
    public class GeolocationGEOTypeFactory : QRCodeTypeFactory
    {
        public GeolocationGEOTypeFactory(ILocalizedTextService localizedTextService) : base(localizedTextService)
        {

        }

        public override string Id => "GeolocationGEO";

        public override IQRCodeType Create(IQRCodeSource qrCodeSource)
        {
            return new GeolocationGEOType(qrCodeSource);
        }
    }
}
