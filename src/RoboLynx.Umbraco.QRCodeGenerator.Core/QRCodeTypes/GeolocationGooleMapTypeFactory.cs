using RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources;
using Umbraco.Cms.Core.Services;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes
{
    public class GeolocationGooleMapTypeFactory : QRCodeTypeFactory
    {
        public GeolocationGooleMapTypeFactory(ILocalizedTextService localizedTextService) : base(localizedTextService)
        {

        }

        public override string Id => "GeolocationGooleMap";

        public override IQRCodeType Create(IQRCodeSource qrCodeSource)
        {
            return new GeolocationGooleMapType(qrCodeSource);
        }
    }
}
