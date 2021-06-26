namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes.Validators
{
    public class NotEmptyValidator : IQRCodeTypeValidator
    {
        bool IQRCodeTypeValidator.Validate(object value, out string message)
        {
            bool isValid;

            if (value is string stringValue)
            {
                isValid = !string.IsNullOrEmpty(stringValue);
            }
            else
            {
                isValid = !(value is null);
            }

            message = !isValid ? "Passed value can not be empty." : null;

            return isValid;
        }
    }
}
