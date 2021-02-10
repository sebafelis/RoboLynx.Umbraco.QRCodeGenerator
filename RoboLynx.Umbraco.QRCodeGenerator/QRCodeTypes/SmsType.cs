using RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes.Validators;
using System.Collections.Generic;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.Services;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes
{
    public class SmsType : QRCodeType
    {
        const string numberArgumentName = "number";
        const string subjectArgumentName = "subject";

        public SmsType(ILocalizedTextService localizedTextService) : base(localizedTextService)
        {
            validators.Add(numberArgumentName, new List<IQRCodeTypeValidator>() { new NotEmptyValidator(), new PhoneNumberValidator() });
            validators.Add(subjectArgumentName, new List<IQRCodeTypeValidator>());
        }

        public override string Id => "SMS";

        public override string Value(IQRCodeSource source, string sourceSettings, IPublishedContent content)
        {
            var number = source.GetValue<string>(0, numberArgumentName, content, sourceSettings);
            RunValidator(numberArgumentName, number);

            var subject = source.GetValue<string>(1, subjectArgumentName, content, sourceSettings);
            RunValidator(subjectArgumentName, subject);

            return new QRCoder.PayloadGenerator.SMS(number, subject).ToString();
        }
    }
}
