using RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes.Validators;
using System;
using System.Collections.Generic;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.Services;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes
{
    public class SmsTypeValue : QRCodeTypeValue
    {
        const string numberArgumentName = "number";
        const string subjectArgumentName = "subject";

        private readonly IQRCodeSource _source;
        private string _number;
        private string _subject;

        public SmsTypeValue(string number, string subject = null) : this()
        {
            if (string.IsNullOrEmpty(number))
            {
                throw new System.ArgumentException($"Argument cannot be null or empty.", nameof(number));
            }

            this._number = number;
            this._subject = subject;
        }

        public SmsTypeValue(IQRCodeSource source) : this()
        {
            _source = source;
        }

        private SmsTypeValue()
        {
            Validators.Add(numberArgumentName, new List<IQRCodeTypeValidator>() { new NotEmptyValidator(), new PhoneNumberValidator() });
            Validators.Add(subjectArgumentName, new List<IQRCodeTypeValidator>());
        }

        public override string Value(bool validate = true)
        {
            _number = _source.GetValue<string>(0, numberArgumentName);
            if (validate)
            {
                Validate(numberArgumentName, _number);
            }

            _subject = _source.GetValue<string>(1, subjectArgumentName);
            if (validate)
            {
                Validate(subjectArgumentName, _subject);
            }

            return new QRCoder.PayloadGenerator.SMS(_number, _subject).ToString();
        }
    }
}
