using RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes.Validators;
using System;
using System.Collections.Generic;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes
{
    public class SmsType : QRCodeType
    {
        const string numberArgumentName = "number";
        const string subjectArgumentName = "subject";

        private readonly IQRCodeSource _source;
        private string _number;
        private string _subject;
        private readonly bool _validate;

        public SmsType(string number, string subject = null, bool validate = true) : this()
        {
            if (string.IsNullOrEmpty(number))
            {
                throw new ArgumentException($"Argument cannot be null or empty.", nameof(number));
            }

            _number = number;
            _subject = subject;
            _validate = validate;
        }

        public SmsType(IQRCodeSource source) : this()
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
            _validate = true;
        }

        private SmsType() : base()
        {
            Validators.Add(numberArgumentName, new List<IQRCodeTypeValidator>() { new NotEmptyValidator(), new PhoneNumberValidator() });
            Validators.Add(subjectArgumentName, new List<IQRCodeTypeValidator>());
        }

        public override string GetCodeContent()
        {
            if (_source is not null)
            {
                _number = _source.GetValue<string>(0, numberArgumentName);
                _subject = _source.GetValue<string>(1, subjectArgumentName);
            }

            if (_validate)
            {
                Validate(numberArgumentName, _number);
                Validate(subjectArgumentName, _subject);
            }

            var result = new QRCoder.PayloadGenerator.SMS(_number, _subject).ToString();
            if (_validate)
            {
                Validate(AllFieldsValidator, result);
            }
            return result;
        }
    }
}
