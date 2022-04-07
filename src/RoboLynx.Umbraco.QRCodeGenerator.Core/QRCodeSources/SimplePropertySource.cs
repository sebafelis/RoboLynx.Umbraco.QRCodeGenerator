using Newtonsoft.Json;
using RoboLynx.Umbraco.QRCodeGenerator.Exceptions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Security;
using Umbraco.Extensions;
using static RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources.SimplePropertySource.SilmplePropertySourceSettings;
using UmbracoConstants = Umbraco.Cms.Core.Constants;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources
{
    public class SimplePropertySource : QRCodeSource
    {
        public class SilmplePropertySourceSettings
        {
            public struct SettingsItem
            {
                public string Name { get; set; }
                public string Regex { get; set; }

                public static implicit operator SettingsItem(string s) => ConvertFromString(s);

                public static implicit operator string(SettingsItem s) => ConvertToString(s);

                private static SettingsItem ConvertFromString(string s)
                {
                    string sn = s, sr = null;
                    var expresion = new Regex(@"(\w*)(?:\{\{(.*)\}\})?", RegexOptions.Singleline);
                    var match = expresion.Match(s);
                    if (match.Success)
                    {
                        sn = match.Groups[1].Value;
                        sr = match.Groups[2].Value;
                    }
                    return new SettingsItem() { Name = sn, Regex = sr };
                }

                private static string ConvertToString(SettingsItem s)
                {
                    if (!string.IsNullOrEmpty(s.Regex))
                        return $"{s.Name}{{{{{s.Regex}}}}}";

                    return s.Name;
                }
            }

            public IDictionary<object, SettingsItem> Properties { get; set; }
        }

        private readonly SilmplePropertySourceSettings _settings;
        private readonly IBackOfficeSecurityAccessor _backOfficeSecurityAccessor;
        private readonly IPublishedValueFallback _publishedValueFallback;
        private readonly IPublishedContent _content;
        private readonly string _culture;

        public SimplePropertySource(IBackOfficeSecurityAccessor backOfficeSecurityAccessor, IPublishedValueFallback publishedValueFallback, IPublishedContent content, string sourceSettings, string culture) : base()
        {
            if (content is null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            if (sourceSettings.DetectIsJson())
            {
                _settings = JsonConvert.DeserializeObject<SilmplePropertySourceSettings>(sourceSettings);
            }
            else
            {
                _settings = new SilmplePropertySourceSettings() { Properties = Regex.Matches(sourceSettings, @"\w+((\{\{).*?(\}\}))?").Cast<Match>().Select((m, i) => new { Key = i, m.Value }).ToDictionary(k => (object)k.Key, v => (SettingsItem)v.Value) };
            }

            if (_settings is null)
            {
                throw new SourceConfigurationQRCodeGeneratorException(GetType(), "QR Code Source is not configure.");
            }
            _backOfficeSecurityAccessor = backOfficeSecurityAccessor;
            _publishedValueFallback = publishedValueFallback;
            _content = content;
            _culture = culture;
        }

        public override T GetValue<T>(int index, string key)
        {
            object propertyValue = null;
            string regexPattern = null;

            var settingItem = GetSettingItem(index, key);
            IPublishedProperty property = _content.GetProperty(settingItem.Name);
            if (property != null)
            {
                if (!property.PropertyType.IsUserProperty
                    && !(property.PropertyType.ContentType.ItemType == PublishedItemType.Member
                    && _backOfficeSecurityAccessor.BackOfficeSecurity.IsAuthenticated()
                    && _backOfficeSecurityAccessor.BackOfficeSecurity.CurrentUser.HasSectionAccess(UmbracoConstants.Applications.Members)))
                {
                    property = null;
                }

                regexPattern = settingItem.Regex;

                if (property is null)
                {
                    throw new SourceConfigurationQRCodeGeneratorException(GetType(), "QR Code Source is not configure correctly.");
                }

                propertyValue = property.Value(_publishedValueFallback, _culture);
            }

            if (propertyValue == null && settingItem.Name.Equals(nameof(_content.Name), StringComparison.OrdinalIgnoreCase))
            {
                propertyValue = _content.Name;
            }

            if (!string.IsNullOrEmpty(regexPattern) && propertyValue is string spropertyValue)
            {
                var regex = new Regex(regexPattern);
                var match = regex.Match(spropertyValue);
                if (match.Success)
                {
                    propertyValue = match.Value;
                }
            }

            try
            {
                return propertyValue != null ? (T)Convert.ChangeType(propertyValue, typeof(T), CultureInfo.InvariantCulture) : default;
            }
            catch (System.FormatException fex)
            {
                throw new SourceConfigurationQRCodeGeneratorException(GetType(), "Property value has wrong format to convert to specify type.", fex);
            }
        }

        private SettingsItem GetSettingItem(int index, string key)
        {
            object settingPropertyIndex = null;
            if (!string.IsNullOrEmpty(key) && (_settings.Properties?.ContainsKey(key) ?? false) && _settings.Properties[key] != null)
            {
                settingPropertyIndex = key;
            }
            else if (_settings.Properties.Count > index && _settings.Properties[index] != null)
            {
                settingPropertyIndex = index;
            }

            return _settings.Properties[settingPropertyIndex];
        }
    }
}