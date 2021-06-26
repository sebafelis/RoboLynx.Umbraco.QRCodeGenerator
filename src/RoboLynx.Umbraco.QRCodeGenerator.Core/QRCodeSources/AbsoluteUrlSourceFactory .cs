﻿using Newtonsoft.Json;
using RoboLynx.Umbraco.QRCodeGenerator.Exceptions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Umbraco.Core;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.Services;
using Umbraco.Web;
using static RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources.SimplePropertySource.SilmplePropertySourceSettings;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources
{
    public class AbsoluteUrlSourceFactory : QRCodeSourceFactory
    {
        public AbsoluteUrlSourceFactory(ILocalizedTextService localizedTextService) : base(localizedTextService)
        {

        }

        public override string Id => "AbsoluteUrl";

        public override IQRCodeSource Create(IPublishedContent publishedContent, string sourceSettings, string culture)
        {
            return new AbsoluteUrlSource(publishedContent, culture);
        }
    }
}

