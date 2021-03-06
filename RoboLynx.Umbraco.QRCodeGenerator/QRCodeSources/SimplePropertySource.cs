using Newtonsoft.Json;
using RoboLynx.Umbraco.QRCodeGenerator.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources
{
    public class SimplePropertySource : QRCodeSource
    {
        public class SilmplePropertySourceSettings
        {
            public IDictionary<object, string> Preperties { get; set; }
        }

        public SimplePropertySource(ILocalizedTextService localizedTextService) : base(localizedTextService)
        {
           
        }

        public override string Id => "SilmpleProperty";

        public override T GetValue<T>(int index, string key, IPublishedContent content, string sourceSettings, string culture)
        {
            if (content is null)
            {
                throw new System.ArgumentNullException(nameof(content));
            }

            SilmplePropertySourceSettings settings;
            if (sourceSettings.DetectIsJson())
            {
                settings = JsonConvert.DeserializeObject<SilmplePropertySourceSettings>(sourceSettings);
            }
            else
            {
                settings = new SilmplePropertySourceSettings() { Preperties = sourceSettings?.Split(',').Select((s, i) => new { key = i, value = s.Trim() }).ToDictionary(k => (object)k.key, v => v.value) };
            }

            IPublishedProperty property = null;
            if (!string.IsNullOrEmpty(key) && settings.Preperties.ContainsKey(key) && settings.Preperties[key] != null)
            {
                property = content.GetProperty(settings.Preperties[key]);
                if (property != null)
                {
                    return (T)Convert.ChangeType(property.Value(culture), typeof(T));
                }
            }
            if (settings.Preperties.Count > index && settings.Preperties[index] != null)
            {
                property = content.GetProperty(settings.Preperties[index]);
            }

            return !(property is null)
                ? (T)Convert.ChangeType(property.Value(culture), typeof(T))
                : throw new SourceConfigurationQRCodeGeneratorException(GetType(), "QR Code Source is not configured correctly.");
        }

    }
}

