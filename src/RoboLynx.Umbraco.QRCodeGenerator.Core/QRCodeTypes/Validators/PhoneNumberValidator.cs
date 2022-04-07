using System.Text.RegularExpressions;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes.Validators
{
    public class PhoneNumberValidator : IQRCodeTypeValidator
    {
        public bool Validate(object value, out string message)
        {
            if (value is string stringValue)
            {
                var isValid = Regex.IsMatch(stringValue, @"^\+?[\d#*]{1,14}$");

                message = !isValid ? "A number is not in correct" : null;

                return isValid;
            }

            message = "Value is not a string";

            return false;
        }
    }
}