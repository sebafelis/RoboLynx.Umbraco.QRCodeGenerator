using RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes.Validators;
using System;
using System.Collections.Generic;
using Umbraco.Core;
using Umbraco.Core.Services;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes
{
    public class SmsType : QRCodeType
    {
        const string numberArgumentName = "number";
        const string subjectArgumentName = "subject";

        public SmsType() : base(null)
        {

        }

        public SmsType(IQRCodeSource source) : base(source)
        {
            validators.Add(numberArgumentName, new List<IQRCodeTypeValidator>() { new NotEmptyValidator(), new PhoneNumberValidator() });
            validators.Add(subjectArgumentName, new List<IQRCodeTypeValidator>());
        }

        public override string Value
        {
            get
            {
                CheckSource();

                var number = source.GetValue<string>(0, numberArgumentName);
                RunValidator(numberArgumentName, number);

                var subject = source.GetValue<string>(1, subjectArgumentName);
                RunValidator(subjectArgumentName, subject);

                return new QRCoder.PayloadGenerator.SMS(number, subject).ToString();
            }
        }
    }
}
