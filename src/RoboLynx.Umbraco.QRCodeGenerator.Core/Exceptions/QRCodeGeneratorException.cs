using System;
using System.Diagnostics.CodeAnalysis;

namespace RoboLynx.Umbraco.QRCodeGenerator.Exceptions
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class QRCodeGeneratorException : Exception
    {
        public QRCodeGeneratorException()
        { }

        public QRCodeGeneratorException(string message) : base(message)
        {
        }

        public QRCodeGeneratorException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}