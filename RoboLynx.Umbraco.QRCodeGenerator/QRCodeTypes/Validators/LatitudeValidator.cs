using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes.Validators
{
    public class LatitudeValidator : IQRCodeTypeValidator
    {
        bool IQRCodeTypeValidator.Validate(object value, out string message)
        {
            double? lat = null;

            if (value is string)
            {
                if (double.TryParse((string)value, System.Globalization.NumberStyles.Any, NumberFormatInfo.InvariantInfo, out double latParsed))
                {
                    lat = latParsed;
                }
                else
                {
                    message = "String value can not to be parse to latitude.";
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
            throw new ArgumentException("Wrong input argument format.", nameof(value));
        }
    }
}
