using RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes.Validators;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.Services;
using static QRCoder.PayloadGenerator;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes
{
    public class PhoneNumberType : QRCodeType
    {
        const string numberArgumentName = "number";

        public PhoneNumberType(ILocalizedTextService localizedTextService) : base(localizedTextService)
        {
            validators.Add(numberArgumentName, new List<IQRCodeTypeValidator>() { new NotEmptyValidator(), new PhoneNumberValidator() });
        }

        public override string Id => "PhoneNumber";

        public override string Value(IQRCodeSource source, string sourceSettings, IPublishedContent content, string culture)
        {
            var number = source.GetValue<string>(0, numberArgumentName, content, sourceSettings, culture);

            number = Regex.Replace(number, @"[\s\(\)-]", string.Empty);

            RunValidator(numberArgumentName, number);

            return new PhoneNumber(number).ToString();
        }
    }
}
