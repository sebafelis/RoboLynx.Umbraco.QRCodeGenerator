using RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes.Validators;
using System;
using System.Collections.Generic;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.Services;
using static QRCoder.PayloadGenerator;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes
{
    public class UrlType : QRCodeType
    {
        const string urlArgumentName = "url";
        private string url;

        public class AbsoluteUrlSourceSettings
        {
            public bool Validate { get; set; } = true;
        }

        public UrlType(string url, ILocalizedTextService localizedTextService = null) : this(localizedTextService)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new System.ArgumentException($"'{nameof(url)}' cannot be null or empty.", nameof(url));
            }

            this.url = url;
        }

        public UrlType(ILocalizedTextService localizedTextService) : base(localizedTextService)
        {
            validators.Add(urlArgumentName, new List<IQRCodeTypeValidator>() { new NotEmptyValidator(), new UrlValidator() });
        }

        public override string Id => "URL";

        public override string Value(IQRCodeSource source, string sourceSettings, IPublishedContent content, string culture, bool validate = true)
        {
            if (url != null)
            {
                throw new InvalidOperationException(message: $"Argument {nameof(url)} was passed in constructor. Is not possible to use source to build type value in this case.");
            }
            url = source.GetValue<string>(0, urlArgumentName, content, sourceSettings, culture);

            if (validate)
            {
                Validate(urlArgumentName, url);
            }

            return new Url(url).ToString();
        }

        public override string Value(bool validate = true)
        {
            if (url == null)
            {
                throw new InvalidOperationException(message: $"Argument {nameof(url)} was not passed in constructor. Is not possible to use value of this argument to build type value in this case.");
            }

            if (validate)
            {
                Validate(urlArgumentName, url);
            }

            return new Url(url).ToString();
        }
    }
}
