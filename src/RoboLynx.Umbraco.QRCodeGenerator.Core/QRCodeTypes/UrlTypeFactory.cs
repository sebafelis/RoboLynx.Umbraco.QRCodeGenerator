using RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes.Validators;
using System;
using System.Collections.Generic;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.Services;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes
{
    public class UrlTypeFactory : QRCodeTypeFactory
    {
        public UrlTypeFactory(ILocalizedTextService localizedTextService) : base(localizedTextService)
        {

        }

        public override string Id => "URL";

        public override IQRCodeType Create(IQRCodeSource qrCodeSource)
        {
            return new UrlType(qrCodeSource);
        }
    }
}
