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

        public GeolocationGooleMapType(ILocalizedTextService localizedTextService) : base(localizedTextService)
        {
            validators.Add(latitudeArgumentName, new List<IQRCodeTypeValidator>() { new NotEmptyValidator(), new LatitudeValidator() });
            validators.Add(longitudeArgumentName, new List<IQRCodeTypeValidator>() { new NotEmptyValidator(), new LongitudeValidator() });
        }

        public override string Id => "GeolocationGooleMap";

        public override string Value(IQRCodeSource source, string sourceSettings, IPublishedContent content)
        {
            if (source is null)
            {
                throw new System.ArgumentNullException(nameof(source));
            }

            var latitude = source.GetValue<double>(0, latitudeArgumentName, content, sourceSettings);
            RunValidator(latitudeArgumentName, latitude);

            var longitude = source.GetValue<double>(1, longitudeArgumentName, content, sourceSettings);
            RunValidator(longitudeArgumentName, longitude);

            var usCultureInfo = new CultureInfo("en-US", false);
            return new PayloadGenerator.Geolocation(latitude.ToString(usCultureInfo), longitude.ToString(usCultureInfo), PayloadGenerator.Geolocation.GeolocationEncoding.GoogleMaps).ToString();
        }
    }
}

