using RoboLynx.Umbraco.QRCodeGenerator.Controllers;
using RoboLynx.Umbraco.QRCodeGenerator.Models;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;
using Umbraco.Core.Models.PublishedContent;

namespace RoboLynx.Umbraco.QRCodeGenerator.Extensions
{
    public class QRCodeBuilder : IQRCodeBuilder
    {
        private readonly QRCodeFormatsCollection formats;
        private readonly QRCodeSourcesCollection sources;
        private readonly QRCodeTypesCollection types;

        public QRCodeBuilder(QRCodeFormatsCollection formats, QRCodeSourcesCollection sources, QRCodeTypesCollection types)
        {
            this.formats = formats;
            this.sources = sources;
            this.types = types;
        }

        public QRCodeSettings GetDefaultSettings(IPublishedContent publishedContent, string propertyAlias)
        {
            return CreateDefaultSettings(GetDataTypePrevalues(publishedContent, propertyAlias));
        }

        public QRCodeConfig CreateConfiguration(IPublishedContent publishedContent, string propertyAlias, QRCodeSettings userSettings)
        {
            var dataTypePrevalue = GetDataTypePrevalues(publishedContent, propertyAlias);

            if (dataTypePrevalue != null)
            {
                var finalSettings = MargeSettings(CreateDefaultSettings(dataTypePrevalue), userSettings);
                var codeSourceSettings = dataTypePrevalue.ContainsKey(Constants.CodeSourceSettingsFieldName) ? dataTypePrevalue[Constants.CodeSourceSettingsFieldName] : string.Empty;
                var codeSource = dataTypePrevalue.ContainsKey(Constants.CodeSourceFieldName) ? (sources.SingleOrDefault(s => s.Id == dataTypePrevalue[Constants.CodeSourceFieldName]) ?? throw new ArgumentException($"{Constants.CodeSourceFieldName} parameter has wrong value.")) : throw new ArgumentException($"{Constants.CodeSourceFieldName} parameter is not set up.");
                var codeType = dataTypePrevalue.ContainsKey(Constants.CodeTypeFieldName) ? (types.SingleOrDefault(t => t.Id == dataTypePrevalue[Constants.CodeTypeFieldName]) ?? throw new ArgumentException($"{Constants.CodeTypeFieldName} parameter has wrong value.")) : throw new ArgumentException($"{Constants.CodeTypeFieldName} parameter is not set up.");
                var codeFormat = formats.SingleOrDefault(f => f.Id == finalSettings.Format) ?? throw new ArgumentException($"{Constants.FormatFieldName} parameter has wrong value.");

                return new QRCodeConfig()
                {
                    Settings = finalSettings,
                    Source = codeSource,
                    Type = codeType,
                    Format = codeFormat,
                    SourceSettings = codeSourceSettings
                };
            }
            return null;
        }

        public HttpContent CreateQRCodeAsResponse(IPublishedContent publishedContent, string propertyAlias, QRCodeSettings userSettings)
        {
            if (publishedContent is null)
            {
                throw new ArgumentNullException(nameof(publishedContent));
            }

            if (propertyAlias is null)
            {
                throw new ArgumentNullException(nameof(propertyAlias));
            }

            var config = CreateConfiguration(publishedContent, propertyAlias, userSettings);
            var value = config.Type.Value(config.Source, config.SourceSettings, publishedContent);

            return config.Format.ResponseContent(value, config.Settings);
        }

        private QRCodeSettings CreateDefaultSettings(IDictionary<string, string> dataTypePrevalue)
        {
            if (dataTypePrevalue is null)
            {
                return null;
            }

            return new QRCodeSettings()
            {
                Size = dataTypePrevalue != null && dataTypePrevalue.ContainsKey(Constants.DefaultSizeFieldName) ? int.Parse(dataTypePrevalue[Constants.DefaultSizeFieldName]) : 20,
                IconSizePercent = dataTypePrevalue != null && dataTypePrevalue.ContainsKey(Constants.DefaultIconSizePercentFieldName) ? int.Parse(dataTypePrevalue[Constants.DefaultIconSizePercentFieldName]) : 5,
                Format = dataTypePrevalue != null && dataTypePrevalue.ContainsKey(Constants.DefaultFormatFieldName) ? dataTypePrevalue[Constants.DefaultFormatFieldName] : formats.First().Id,
                DarkColor = dataTypePrevalue != null && dataTypePrevalue.ContainsKey(Constants.DefaultDarkColorFieldName) ? dataTypePrevalue[Constants.DefaultDarkColorFieldName] : "#000000",
                LightColor = dataTypePrevalue != null && dataTypePrevalue.ContainsKey(Constants.DefaultLightColorFieldName) ? dataTypePrevalue[Constants.DefaultLightColorFieldName] : "#FFFFFF",
                DrawQuiteZone = dataTypePrevalue != null && dataTypePrevalue.ContainsKey(Constants.DefaultDrawQuietZoneFieldName) && (dataTypePrevalue[Constants.DefaultDrawQuietZoneFieldName]?.StringToBoolean(false) ?? false),
                IconBorderWidth = dataTypePrevalue != null && dataTypePrevalue.ContainsKey(Constants.DefaultIconBorderWidthFieldName) ? int.Parse(dataTypePrevalue[Constants.DefaultIconBorderWidthFieldName]) : 2,
                Icon = dataTypePrevalue != null && dataTypePrevalue.ContainsKey(Constants.DefaultIconFieldName) ? dataTypePrevalue[Constants.DefaultIconFieldName] : null,
                ECCLevel = dataTypePrevalue != null && dataTypePrevalue.ContainsKey(Constants.DefaultECCLevelFieldName) ? (ECCLevel)Enum.Parse(typeof(ECCLevel), dataTypePrevalue[Constants.DefaultECCLevelFieldName]) : ECCLevel.L
            };
        }

        private IDictionary<string, string> GetDataTypePrevalues(IPublishedContent publishedContent, string propertyAlias)
        {
            var dataType = publishedContent.GetProperty(propertyAlias)?.PropertyType.DataType;
            if (dataType?.EditorAlias == Constants.PropertyEditorAlias && dataType.Configuration != null)
            {
                var configuration = ((IEnumerable<KeyValuePair<string, object>>)dataType.Configuration)?.ToDictionary(k => k.Key, v => v.Value?.ToString());

                return configuration;
            }
            return null;
        }

        private QRCodeSettings MargeSettings(QRCodeSettings defaultSettings, QRCodeSettings userSettings)
        {
            if (defaultSettings is null)
            {
                throw new ArgumentNullException(nameof(defaultSettings));
            }

            var settings = (QRCodeSettings)defaultSettings.Clone();

            if (userSettings != null)
            {
                foreach (var settingsProperty in typeof(QRCodeSettings).GetAllProperties())
                {
                    var propertyValue = settingsProperty.GetValue(userSettings);
                    var defaultPropertyValue = settingsProperty.PropertyType.GetDefaultValue();
                    if ((propertyValue != null && !propertyValue.Equals(defaultPropertyValue)) || (propertyValue == null && propertyValue != defaultPropertyValue))
                    {
                        settingsProperty.SetValue(settings, propertyValue);
                    }
                }
            }
            return settings;
        }
    }
}
