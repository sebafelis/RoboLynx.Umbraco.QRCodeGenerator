using QRCoder;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes.Validators;
using System;
using System.Collections.Generic;
using Umbraco.Core;
using Umbraco.Core.Services;
using static QRCoder.PayloadGenerator;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes
{
    public class PhoneNumberType : QRCodeType
    {
        const string numberArgumentName = "number";

        public PhoneNumberType() : base(null)
        {

        }

        public PhoneNumberType(IQRCodeSource source) : base(source)
        {
            validators.Add(numberArgumentName, new List<IQRCodeTypeValidator>() { new NotEmptyValidator(), new PhoneNumberValidator() });
        }

        public override string Value
        {
            get
            {
                CheckSource();

                var number = source.GetValue<string>(0, numberArgumentName);
                RunValidator(numberArgumentName, number);

                return new QRCoder.PayloadGenerator.PhoneNumber(number).ToString();
            }
        }
    }
}
