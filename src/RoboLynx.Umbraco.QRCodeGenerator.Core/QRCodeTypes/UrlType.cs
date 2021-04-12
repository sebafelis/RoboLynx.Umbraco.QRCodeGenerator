using RoboLynx.Umbraco.QRCodeGenerator.Exceptions;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes.Validators;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Services;
using static QRCoder.PayloadGenerator;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes
{
    public class UrlType : QRCodeType
    {
        const string urlArgumentName = "url";

        public class AbsoluteUrlSourceSettings
        {
            public bool Validate { get; set; } = true;
        }

        public UrlType() : base(null)
        {

        }

        public UrlType(IQRCodeSource source): base(source)
        {
            validators.Add(urlArgumentName, new List<IQRCodeTypeValidator>() { new NotEmptyValidator(), new UrlValidator() });
        }

        public override string Value
        {
            get
            {
                CheckSource();

                var url = source.GetValue<string>(0, urlArgumentName);
                RunValidator(urlArgumentName, url);

                return new Url(url).ToString();
            }
        }
    }
}
