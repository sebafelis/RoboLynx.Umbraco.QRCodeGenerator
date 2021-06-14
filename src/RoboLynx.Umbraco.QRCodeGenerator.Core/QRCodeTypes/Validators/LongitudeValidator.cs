using System;
using System.Globalization;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes.Validators
{
    public class LongitudeValidator : IQRCodeTypeValidator
    {
        bool IQRCodeTypeValidator.Validate(object value, out string message)
        {
            double? lng = null;

            if (value is string stringValue)
            {
                if (double.TryParse(stringValue, System.Globalization.NumberStyles.Any, NumberFormatInfo.InvariantInfo, out double latParsed))
                {
                    lng = latParsed;
                }
                else
                {
                    message = "String value can not to be parse to longitude.";
                    return false;
                }
            }
            else
            {
                try
                {
                    lng = (double?)Convert.ChangeType(value, typeof(double));
                }
                catch (InvalidCastException)
                {

                }
            }

            if (lng.HasValue)
            {
                var isLongitude = Math.Abs(lng.Value) <= 180;
                if (!isLongitude)
                {
                    message = "Value is grate or less to be valid longitude.";
                }
                else
                {
                    message = null;
                }
                return isLongitude;
            }

            message = "Unsupported input.";
            return false;
        }
    }
}
