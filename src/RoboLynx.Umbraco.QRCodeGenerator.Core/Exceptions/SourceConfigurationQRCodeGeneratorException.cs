using System;
using System.Diagnostics.CodeAnalysis;

namespace RoboLynx.Umbraco.QRCodeGenerator.Exceptions
{
    [System.Serializable]
    [ExcludeFromCodeCoverage]
    public class SourceConfigurationQRCodeGeneratorException : QRCodeGeneratorException
    {
        public SourceConfigurationQRCodeGeneratorException(Type sourceType, string message) : base($"Source provider: {sourceType.Name}; {message}")
        {
        }

        public SourceConfigurationQRCodeGeneratorException(Type sourceType, string message, Exception inner) : base($"Source provider: {sourceType.Name}; {message}", inner)
        {
        }

        public SourceConfigurationQRCodeGeneratorException(Type sourceType, string attributeName, string message) : base($"Source provider: {sourceType.Name}; Source provider interface: {attributeName};  {message}")
        {
        }

        public SourceConfigurationQRCodeGeneratorException(Type sourceType, string attributeName, string message, Exception inner) : base($"Source provider: {sourceType.Name}; Source provider interface: {attributeName}; {message}", inner)
        {
        }

        protected SourceConfigurationQRCodeGeneratorException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}