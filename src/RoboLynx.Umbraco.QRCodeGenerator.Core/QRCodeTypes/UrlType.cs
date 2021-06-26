using RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes.Validators;
using System;
using System.Collections.Generic;
using static QRCoder.PayloadGenerator;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes
{
    public class UrlType : QRCodeType
    {
        const string UrlArgumentName = "url";

        private readonly IQRCodeSource _source;
        private string _url;
        private readonly bool _validate;

        public class AbsoluteUrlSourceSettings
        {
            public bool Validate { get; set; } = true;
        }

        public UrlType(string url, bool validate = true) : this()
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new System.ArgumentException($"'{nameof(url)}' cannot be null or empty.", nameof(url));
            }

            _url = url; 
            _validate = validate;
        }

        public UrlType(IQRCodeSource source)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
            _validate = true;
        }

        public UrlType() : base()
        {
            Validators.Add(UrlArgumentName, new List<IQRCodeTypeValidator>() { new NotEmptyValidator(), new UrlValidator() });
        }

        public override string GetCodeContent()
        {
            if (_source is not null)
            {
                _url = _source.GetValue<string>(0, UrlArgumentName);
            }

            if (_validate)
            {
                Validate(UrlArgumentName, _url);
            }

            var result = new Url(_url).ToString();
            if (_validate)
            {
                Validate(AllFieldsValidator, result);
            }
            return result;
        }
    }
}
