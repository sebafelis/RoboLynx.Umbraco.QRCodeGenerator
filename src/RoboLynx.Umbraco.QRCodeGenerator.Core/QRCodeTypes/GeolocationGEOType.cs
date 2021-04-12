using QRCoder;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes.Validators;
using System.Collections.Generic;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes
{
    public class GeolocationGEOType : QRCodeType
    {
        const string latitudeArgumentName = "latitude";
        const string longitudeArgumentName = "longitude";

        public GeolocationGEOType() : base(null)
        {

        }

        public GeolocationGEOType(IQRCodeSource source) : base(source)
        {
            validators.Add(latitudeArgumentName, new List<IQRCodeTypeValidator>() { new NotEmptyValidator(), new LatitudeValidator() });
            validators.Add(longitudeArgumentName, new List<IQRCodeTypeValidator>() { new NotEmptyValidator(), new LongitudeValidator() });
        }

        public override string Value
        {
            get
            {
                CheckSource();

                var latitude = source.GetValue<string>(0, latitudeArgumentName);
                RunValidator(latitudeArgumentName, latitude);

                var longitude = source.GetValue<string>(1, longitudeArgumentName);
                RunValidator(longitudeArgumentName, longitude);

                return new QRCoder.PayloadGenerator.Geolocation(latitude, longitude, PayloadGenerator.Geolocation.GeolocationEncoding.GEO).ToString();
            }
        }
    }
}
