﻿using RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes.Validators;
using System.Collections.Generic;
using Umbraco.Core.Models.PublishedContent;
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

        public UrlType(ILocalizedTextService localizedTextService) : base(localizedTextService)
        {
            validators.Add(urlArgumentName, new List<IQRCodeTypeValidator>() { new NotEmptyValidator(), new UrlValidator() });
        }

        public override string Id => "URL";

        public override string Value(IQRCodeSource source, string sourceSettings, IPublishedContent content, string culture)
        {
            var url = source.GetValue<string>(0, urlArgumentName, content, sourceSettings, culture);
            RunValidator(urlArgumentName, url);

            return new Url(url).ToString();
        }
    }
}
