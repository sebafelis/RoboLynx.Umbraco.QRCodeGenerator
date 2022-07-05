using RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes.Validators;
using System;
using System.Collections.Generic;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes
{
    public class TextType : QRCodeType
    {
        private const string TextArgumentName = "text";

        private string? _text;
        private readonly IQRCodeSource? _source;
        private readonly bool _validate;

        public TextType(string text, bool validate = true) : this()
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentException($"'{nameof(text)}' cannot be null or empty.", nameof(text));
            }

            _text = text;
            _validate = validate;
        }

        public TextType(IQRCodeSource source) : this()
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
            _validate = true;
        }

        private TextType() : base()
        {
            Validators.Add(TextArgumentName, new List<IQRCodeTypeValidator>() { new NotEmptyValidator() });
        }

        public override string GetCodeContent()
        {
            if (_source is not null)
            {
                _text = _source.GetValue<string>(0, TextArgumentName);
            }

            if (_validate)
            {
                Validate(TextArgumentName, _text);
                Validate(AllFieldsValidator, _text);
            }
            return _text ?? string.Empty;
        }
    }
}