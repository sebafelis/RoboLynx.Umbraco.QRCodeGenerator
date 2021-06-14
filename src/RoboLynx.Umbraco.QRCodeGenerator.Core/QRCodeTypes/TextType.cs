using RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources;
using System;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.Services;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes
{
    public class TextType : QRCodeType
    {
        const string textArgumentName = "text";
        private string text;

        public TextType(string text, ILocalizedTextService localizedTextService = null) : this(localizedTextService)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new System.ArgumentException($"'{nameof(text)}' cannot be null or empty.", nameof(text));
            }

            this.text = text;
        }

        public TextType(ILocalizedTextService localizedTextService) : base(localizedTextService)
        {

        }

        public override string Id => "Text";

        public override string Value(bool validate = true)
        {
            if (validate)
            {
                Validate(textArgumentName, text);
            }

            return text; 
        }
    }
}
