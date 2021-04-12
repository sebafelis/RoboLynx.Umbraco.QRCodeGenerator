using QRCoder;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes.Validators;
using System.Collections.Generic;
using System.Globalization;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes
{
    public class GeolocationGooleMapType : QRCodeType
    {
        const string latitudeArgumentName = "latitude";
        const string longitudeArgumentName = "longitude";

        public GeolocationGooleMapType() : base(null)
        {

        }

        public GeolocationGooleMapType(IQRCodeSource source) : base(source)
        {
            validators.Add(latitudeArgumentName, new List<IQRCodeTypeValidator>() { new NotEmptyValidator(), new LatitudeValidator() });
            validators.Add(longitudeArgumentName, new List<IQRCodeTypeValidator>() { new NotEmptyValidator(), new LongitudeValidator() });
        }

        public override string Value
        {
            get
            {
                CheckSource();

                var latitude = source.GetValue<double>(0, latitudeArgumentName);
                RunValidator(latitudeArgumentName, latitude);

                var longitude = source.GetValue<double>(1, longitudeArgumentName);
                RunValidator(longitudeArgumentName, longitude);

                var usCultureInfo = new CultureInfo("en-US", false);
                return new QRCoder.PayloadGenerator.Geolocation(latitude.ToString(usCultureInfo), longitude.ToString(usCultureInfo), PayloadGenerator.Geolocation.GeolocationEncoding.GoogleMaps).ToString();
            }
        }
    }
}
