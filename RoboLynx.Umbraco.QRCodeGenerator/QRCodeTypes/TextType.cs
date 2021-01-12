using QRCoder;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources;
using System;
using Umbraco.Core;
using Umbraco.Core.Services;
using static QRCoder.PayloadGenerator;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes
{
    public class TextType : QRCodeType
    {
        public TextType() : base(null)
        {

        }

        public TextType(IQRCodeSource source) : base(source)
        {

        }

        public override string Value
        {
            get
            {
                CheckSource();

                return source.GetValue<string>(0, "text");
            }
        }
    }
}
