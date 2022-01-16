using RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes.Validators;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using static QRCoder.PayloadGenerator;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes
{
    public class PhoneNumberType : QRCodeType
    {
        const string NumberArgumentName = "number";

        private readonly IQRCodeSource _source;
        private string _number;
        private readonly bool _validate;

        public PhoneNumberType(string number, bool validate = true) : this()
        {
            _number = number;
            _validate = validate;
        }

        public PhoneNumberType(IQRCodeSource source)
        {
            _source = source;
            _validate = true;
        }

        private PhoneNumberType() : base()
        {
            Validators.Add(NumberArgumentName, new List<IQRCodeTypeValidator>() { new NotEmptyValidator(), new PhoneNumberValidator() });
        }

        public override string GetCodeContent()
        {
            if (_source is not null)
            {
                _number = _source.GetValue<string>(0, NumberArgumentName);
            }

            var number = Regex.Replace(_number, @"[\s\(\)-]", string.Empty);

            if (_validate)
            {
                Validate(NumberArgumentName, number);
            }

            var result = new PhoneNumber(number).ToString();
            if (_validate)
            {
                Validate(AllFieldsValidator, result);
            }
            return result;
        }
    }
}
