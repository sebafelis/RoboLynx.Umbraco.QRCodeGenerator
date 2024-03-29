﻿using System;
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

        protected QRCodeGeneratorException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}