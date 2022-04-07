namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes.Validators
{
    public interface IQRCodeTypeValidator
    {
        bool Validate(object value, out string message);
    }
}