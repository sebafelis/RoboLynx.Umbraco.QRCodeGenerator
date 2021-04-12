using System;

namespace RoboLynx.Umbraco.QRCodeGenerator.Exceptions
{
    [System.Serializable]
    public class ValidationQRCodeGeneratorException : QRCodeGeneratorException
    {
        public ValidationQRCodeGeneratorException(Type qrCodeType, string argument, string message) : base($"Source provider: {qrCodeType.Name}; {message}") { }
        public ValidationQRCodeGeneratorException(Type qrCodeType, string argument, string message, Exception inner) : base($"Source provider: {qrCodeType.Name}; {message}", inner) { }
        protected ValidationQRCodeGeneratorException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

}
