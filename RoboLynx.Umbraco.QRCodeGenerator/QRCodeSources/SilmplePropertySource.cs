using Newtonsoft.Json;
using RoboLynx.Umbraco.QRCodeGenerator.Exceptions;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Models;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources
{
    public class SilmplePropertySource : QRCodeSource
    {
        public class SilmplePropertySourceSettings
        {
            public IDictionary<object, string> Preperties { get; set; }
        }

        private readonly SilmplePropertySourceSettings sourceSettings;

        public SilmplePropertySource() : base(null)
        {

        }

        public SilmplePropertySource(IPublishedContent content, string sourceSettings) : base(content)
        {
            if (sourceSettings.DetectIsJson())
            {
                this.sourceSettings = JsonConvert.DeserializeObject<SilmplePropertySourceSettings>(sourceSettings);
            }
            else
            {
                this.sourceSettings = new SilmplePropertySourceSettings() { Preperties = sourceSettings?.Split(',').Select((s, i) => new { key = i, value = s.Trim() }).ToDictionary(k => (object)k.key, v => v.value) };
            }
        }

        public override T GetValue<T>(int index, string key)
        {
            CheckContent();

            IPublishedProperty property = null;
            if (!string.IsNullOrEmpty(key) && sourceSettings.Preperties.ContainsKey(key) && sourceSettings.Preperties[key] != null)
            {
                property = content.GetProperty(sourceSettings.Preperties[key]);
            }
            if (sourceSettings.Preperties.Count < index && sourceSettings.Preperties[index] != null)
            {
                property = content.GetProperty(sourceSettings.Preperties[index]);
            }

            return !(property is null)
                ? (T)property.Value
                : throw new SourceConfigurationQRCodeGeneratorException(GetType(), "QR Code Source is not configured correctly.");
        }

    }
}

