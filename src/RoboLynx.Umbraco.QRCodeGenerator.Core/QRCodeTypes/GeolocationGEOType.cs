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
        const string _latitudeArgumentName = "latitude";
        const string _longitudeArgumentName = "longitude";

        public GeolocationGEOType(ILocalizedTextService localizedTextService) : base(localizedTextService)
        {
            validators.Add(_latitudeArgumentName, new List<IQRCodeTypeValidator>() { new NotEmptyValidator(), new LatitudeValidator() });
            validators.Add(_longitudeArgumentName, new List<IQRCodeTypeValidator>() { new NotEmptyValidator(), new LongitudeValidator() });
        }

        public override string Id => "GeolocationGEO";

        public override string Value(IQRCodeSource source, string sourceSettings, IPublishedContent content, string culture, bool validate = true)
        {
            if (source is null)
            {
                throw new System.ArgumentNullException(nameof(source));
            }

            if (latitude != null)
            {
                throw new InvalidOperationException(message: $"Argument {nameof(url)} was passed in constructor. Is not possible to use source to build type value in this case.");
            }

            

            var latitude = source.GetValue<string>(0, _latitudeArgumentName, content, sourceSettings, culture);
            if (validate)
            {
                Validate(_latitudeArgumentName, latitude);
            }

            var longitude = source.GetValue<string>(1, _longitudeArgumentName, content, sourceSettings, culture);
            if (validate)
            {
                Validate(_longitudeArgumentName, longitude);
            }

            return new PayloadGenerator.Geolocation(latitude, longitude, PayloadGenerator.Geolocation.GeolocationEncoding.GEO).ToString();
        }

        public override string Value(bool validate = true)
        {
            throw new System.NotImplementedException();
        }
    }
}
