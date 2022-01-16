using QRCoder;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes.Validators;
using System;
using System.Collections.Generic;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes
{
    public class GeolocationGooleMapType : QRCodeType
    {
        const string LatitudeArgumentName = "latitude";
        const string LongitudeArgumentName = "longitude";

        private readonly IQRCodeSource _source;
        private string _latitude;
        private string _longitude;
        private readonly bool _validate;

        public GeolocationGooleMapType(string latitude, string longitude, bool validate = true) : this()
        {
            if (string.IsNullOrEmpty(latitude))
            {
                throw new ArgumentException($"'{nameof(latitude)}' cannot be null or empty.", nameof(latitude));
            }

            if (string.IsNullOrEmpty(longitude))
            {
                throw new ArgumentException($"'{nameof(longitude)}' cannot be null or empty.", nameof(longitude));
            }

            _latitude = latitude;
            _longitude = longitude;
            _validate = validate;
        }

        public GeolocationGooleMapType(IQRCodeSource source) : this()
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
            _validate = true;
        }

        private GeolocationGooleMapType() : base()
        {
            Validators.Add(LatitudeArgumentName, new List<IQRCodeTypeValidator>() { new NotEmptyValidator(), new LatitudeValidator() });
            Validators.Add(LongitudeArgumentName, new List<IQRCodeTypeValidator>() { new NotEmptyValidator(), new LongitudeValidator() });
        }

        public override string GetCodeContent()
        {
            if (_source is not null)
            {
                _latitude = _source.GetValue<string>(0, LatitudeArgumentName);
                _longitude = _source.GetValue<string>(1, LongitudeArgumentName);
            }

            if (_validate)
            {
                Validate(LatitudeArgumentName, _latitude);
                Validate(LongitudeArgumentName, _longitude);
            }

            var result = new PayloadGenerator.Geolocation(_latitude, _longitude, PayloadGenerator.Geolocation.GeolocationEncoding.GoogleMaps).ToString();
            if (_validate)
            {
                Validate(AllFieldsValidator, result);
            }

            return result;
        }
    }
}

