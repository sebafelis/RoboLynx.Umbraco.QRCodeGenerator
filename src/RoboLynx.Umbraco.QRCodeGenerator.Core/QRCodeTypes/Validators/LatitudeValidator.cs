using System;
using System.Globalization;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes.Validators
{
    public class LatitudeValidator : IQRCodeTypeValidator
    {
        bool IQRCodeTypeValidator.Validate(object value, out string message)
        {
            double? lat = null;

            if (value is string stringValue)
            {
                if (double.TryParse(stringValue, NumberStyles.Any, NumberFormatInfo.InvariantInfo, out double latParsed))
                {
                    lat = latParsed;
                }
                else
                {
                    message = "String value can not to be parse to latitude.";
                    return false;
                }
            }
            else
            {
                try
                {
                    lat = (double?)Convert.ChangeType(value, typeof(double));
                }
                catch (InvalidCastException)
                {

                }
            }

            if (lat.HasValue)
            {
                var isLatitude = Math.Abs(lat.Value) <= 90;
                if (!isLatitude)
                {
                    message = "Value is grate or less to be valid latitude.";
                }
                else
                {
                    message = null;
                }
                return isLatitude;
            }

            message = "Unsupported input.";
            return false;
        }
    }
}
