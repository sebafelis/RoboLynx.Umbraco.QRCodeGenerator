using QRCoder;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes.Validators;
using System.Collections.Generic;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.Services;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes
{
    public class GeolocationGEOType : QRCodeType
    {
        const string latitudeArgumentName = "latitude";
        const string longitudeArgumentName = "longitude";

        public GeolocationGEOType(ILocalizedTextService localizedTextService) : base(localizedTextService)
        {
            validators.Add(latitudeArgumentName, new List<IQRCodeTypeValidator>() { new NotEmptyValidator(), new LatitudeValidator() });
            validators.Add(longitudeArgumentName, new List<IQRCodeTypeValidator>() { new NotEmptyValidator(), new LongitudeValidator() });
        }

        public override string Id => "GeolocationGEO";

        public override string Value(IQRCodeSource source, string sourceSettings, IPublishedContent content)
        {
            if (source is null)
            {
                throw new System.ArgumentNullException(nameof(source));
            }

            var latitude = source.GetValue<string>(0, latitudeArgumentName, content, sourceSettings);
            RunValidator(latitudeArgumentName, latitude);

            var longitude = source.GetValue<string>(1, longitudeArgumentName, content, sourceSettings);
            RunValidator(longitudeArgumentName, longitude);

            return new PayloadGenerator.Geolocation(latitude, longitude, PayloadGenerator.Geolocation.GeolocationEncoding.GEO).ToString();
        }
    }
}
