namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes
{
    public interface IQRCodeType
    {
        /// <summary>
        /// Get value of QR Code content by using value passed into constructor.
        /// </summary>
        /// <returns>Code content</returns>
        /// <exception cref="RoboLynx.Umbraco.QRCodeGenerator.Exceptions.ValidationQRCodeGeneratorException">Throw when value passed in constructor is invalid.</exception>
        string GetCodeContent();
    }
}
