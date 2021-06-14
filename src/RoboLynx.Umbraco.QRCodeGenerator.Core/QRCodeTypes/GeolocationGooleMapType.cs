using QRCoder;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes.Validators;
using System.Collections.Generic;
using System.Globalization;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.Services;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes
{
    public class GeolocationGooleMapType : QRCodeType
    {
        const string latitudeArgumentName = "latitude";
        const string longitudeArgumentName = "longitude";
        private readonly IQRCodeSource _source;

        public GeolocationGooleMapType(ILocalizedTextService localizedTextService, IQRCodeSource source) : base(localizedTextService)
        {
            validators.Add(latitudeArgumentName, new List<IQRCodeTypeValidator>() { new NotEmptyValidator(), new LatitudeValidator() });
            validators.Add(longitudeArgumentName, new List<IQRCodeTypeValidator>() { new NotEmptyValidator(), new LongitudeValidator() });
            
            _source = source ?? throw new System.ArgumentNullException(nameof(source));
        }

        public override string Id => "GeolocationGooleMap";

        public override string Value( bool validate = true)
        {
            var latitude = _source.GetValue<double>(0, latitudeArgumentName);
            if (validate)
                Validate(latitudeArgumentName, latitude);

            var longitude = _source.GetValue<double>(1, longitudeArgumentName);
            if (validate)
                Validate(longitudeArgumentName, longitude);

            var usCultureInfo = new CultureInfo("en-US", false);
            return new PayloadGenerator.Geolocation(latitude.ToString(usCultureInfo), longitude.ToString(usCultureInfo), PayloadGenerator.Geolocation.GeolocationEncoding.GoogleMaps).ToString();
        }

        public override string Value(bool validate = true)
        {
            throw new System.NotImplementedException();
        }
    }
}

