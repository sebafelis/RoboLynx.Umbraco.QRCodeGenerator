using System;

namespace RoboLynx.Umbraco.QRCodeGenerator.Exceptions
{
    [System.Serializable]
    public class InvalidSettingQRCodeGeneratorException : QRCodeGeneratorException
    {
        public InvalidSettingQRCodeGeneratorException(string settingName) : base($"Attribute: {settingName}") { }
        public InvalidSettingQRCodeGeneratorException(string settingName, string message) : base($"Attribute: {settingName}; {message}") { }
        public InvalidSettingQRCodeGeneratorException(string settingName, string message, Exception inner) : base($"Attribute: {settingName}; {message}", inner) { }
    }

}
