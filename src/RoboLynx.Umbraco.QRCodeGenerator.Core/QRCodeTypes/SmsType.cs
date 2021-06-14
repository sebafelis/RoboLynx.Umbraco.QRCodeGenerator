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
    public class SmsType : QRCodeType
    {
        public SmsType(ILocalizedTextService localizedTextService) : base(localizedTextService)
        {
        }

        public override string Id => "SMS";

        public override IQRCodeTypeValueFactory CreateValueFactory(IFactory factory)
        {
            return new SmsTypeValue(factory.GetInstance<IQRCodeSource>());
        }
    }
}
