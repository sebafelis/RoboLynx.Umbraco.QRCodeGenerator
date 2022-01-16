using RoboLynx.Umbraco.QRCodeGenerator.Cache;
using RoboLynx.Umbraco.QRCodeGenerator.Models;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace RoboLynx.Umbraco.QRCodeGenerator
{
    internal class QRCodeBuilder : IQRCodeBuilder
    {
        private readonly QRCodeFormatFactoryCollection _formats;
        private readonly QRCodeSourceFactoryCollection _sources;
        private readonly QRCodeTypeFactoryCollection _types;

        public IQRCodeCacheManager CacheManager { get; }

        public QRCodeBuilder(QRCodeFormatFactoryCollection formats, QRCodeSourceFactoryCollection sources, QRCodeTypeFactoryCollection types, IQRCodeCacheManager cacheManager)
        {
            _formats = formats ?? throw new ArgumentNullException(nameof(formats));
            _sources = sources ?? throw new ArgumentNullException(nameof(sources));
            _types = types ?? throw new ArgumentNullException(nameof(types));
            CacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
        }

        public IQRCodeFormat GetFormat(IQRCodeType codeType, QRCodeSettings settings)
        {
            return _formats.FirstOrDefault(f => f.Id == settings.Format).Create(codeType, settings) ?? throw new ArgumentException($"{Constants.FieldsNames.FormatFieldName} parameter has wrong value.");
        }

        private IQRCodeSource GetSource(string id, IPublishedContent publishedContent, string culture, string sourceSettings)
        {
            return _sources.FirstOrDefault(f => f.Id == id).Create(publishedContent, sourceSettings, culture) ?? throw new ArgumentException($"{Constants.FieldsNames.FormatFieldName} parameter has wrong value.");
        }

        private IQRCodeType GetType(string id, IQRCodeSource qtCodeSource)
        {
            return _types.FirstOrDefault(f => f.Id == id).Create(qtCodeSource) ?? throw new ArgumentException($"{Constants.FieldsNames.FormatFieldName} parameter has wrong value.");
        }

        public QRCodeSettings GetDefaultSettings(IPublishedContent publishedContent, string propertyAlias)
        {
            return CreateDefaultSettings(GetDataTypePrevalues(publishedContent, propertyAlias));
        }

        public QRCodeConfig CreateConfiguration(IPublishedContent publishedContent, string propertyAlias, string culture, QRCodeSettings userSettings)
        {
            if (publishedContent is null)
            {
                return null;
            }

            var dataTypePrevalue = GetDataTypePrevalues(publishedContent, propertyAlias);

            if (dataTypePrevalue != null)
            {
                var finalSettings = MargeSettings(CreateDefaultSettings(dataTypePrevalue), userSettings);
                var codeSourceSettings = dataTypePrevalue.ContainsKey(Constants.FieldsNames.CodeSourceSettingsFieldName) ? dataTypePrevalue[Constants.FieldsNames.CodeSourceSettingsFieldName] : string.Empty;
                var codeSource = dataTypePrevalue.ContainsKey(Constants.FieldsNames.CodeSourceFieldName) ? ((GetSource(dataTypePrevalue[Constants.FieldsNames.CodeSourceFieldName], publishedContent, culture, codeSourceSettings)) ?? throw new ArgumentException($"{Constants.FieldsNames.CodeSourceFieldName} parameter has wrong value.")) : throw new ArgumentException($"{Constants.FieldsNames.CodeSourceFieldName} parameter is not set up.");
                var codeType = dataTypePrevalue.ContainsKey(Constants.FieldsNames.CodeTypeFieldName) ? ((GetType(dataTypePrevalue[Constants.FieldsNames.CodeTypeFieldName], codeSource)) ?? throw new ArgumentException($"{Constants.FieldsNames.CodeTypeFieldName} parameter has wrong value.")) : throw new ArgumentException($"{Constants.FieldsNames.CodeTypeFieldName} parameter is not set up.");
                var codeFormat = GetFormat(codeType, finalSettings);

                CompleteSettings(ref finalSettings);

                return new QRCodeConfig()
                {
                    Settings = finalSettings,
                    Source = codeSource,
                    Type = codeType,
                    Format = codeFormat
                };
            }
            return null;
        }

        public QRCodeConfig CreateConfiguration(IQRCodeType codeType, QRCodeSettings userSettings)
        {
            CompleteSettings(ref userSettings);

            var codeFormat = GetFormat(codeType, userSettings);

            return new QRCodeConfig()
            {
                Settings = userSettings,
                Source = null,
                Type = codeType,
                Format = codeFormat
            };
        }

        private static void CompleteSettings(ref QRCodeSettings userSettings)
        {
            if (string.IsNullOrEmpty(userSettings.DarkColor))
                userSettings.DarkColor = Constants.DefaultFieldsValues.DefaultDarkColorFieldValue;

            if (string.IsNullOrEmpty(userSettings.LightColor))
                userSettings.DarkColor = Constants.DefaultFieldsValues.DefaultLightColorFieldValue;

            if (userSettings.Size <= 0)
                userSettings.Size = Constants.DefaultFieldsValues.DefaultSizeFieldValue;

            if (!userSettings.ECCLevel.HasValue)
                userSettings.ECCLevel = Constants.DefaultFieldsValues.DefaultECCLevelFieldValue;

            if (!userSettings.DrawQuiteZone.HasValue)
                userSettings.DrawQuiteZone = Constants.DefaultFieldsValues.DefaultDrawQuietZoneFieldValue;

            if (userSettings.IconSizePercent <= 0)
                userSettings.IconSizePercent = Constants.DefaultFieldsValues.DefaultIconSizePercentFieldValue;

            if (userSettings.IconBorderWidth < 0)
                userSettings.IconBorderWidth = Constants.DefaultFieldsValues.DefaultIconBorderWidthFieldValue;

            if (string.IsNullOrEmpty(userSettings.Format))
                userSettings.Format = Constants.DefaultFieldsValues.DefaultFormatFieldValue;
        }

        public Stream CreateStream(QRCodeConfig config, string cacheName)
        {
            var hashId = config.Format.HashId;
            var extension = config.Format.FileExtension;

            if (CacheManager.IsCached(hashId, cacheName))
            {
                var stream = CacheManager.GetStream(hashId, cacheName);

                return stream;
            }
            else
            {
                var qrCodeStream = config.Format.Stream();
                qrCodeStream.Seek(0, SeekOrigin.Begin);

                CacheManager.Add(hashId, extension, qrCodeStream, cacheName);

                qrCodeStream.Seek(0, SeekOrigin.Begin);
                return qrCodeStream;
            }
        }

        private static QRCodeSettings CreateDefaultSettings(IDictionary<string, string> dataTypePrevalue)
        {
            if (dataTypePrevalue is null)
            {
                return null;
            }

            var settings = new QRCodeSettings()
            {
                Size = dataTypePrevalue != null && dataTypePrevalue.ContainsKey(Constants.DefaultFieldsNames.DefaultSizeFieldName) ? int.Parse(dataTypePrevalue[Constants.DefaultFieldsNames.DefaultSizeFieldName]) : 0,
                IconSizePercent = dataTypePrevalue != null && dataTypePrevalue.ContainsKey(Constants.DefaultFieldsNames.DefaultIconSizePercentFieldName) ? int.Parse(dataTypePrevalue[Constants.DefaultFieldsNames.DefaultIconSizePercentFieldName]) : 0,
                Format = dataTypePrevalue != null && dataTypePrevalue.ContainsKey(Constants.DefaultFieldsNames.DefaultFormatFieldName) ? dataTypePrevalue[Constants.DefaultFieldsNames.DefaultFormatFieldName] : null,
                DarkColor = dataTypePrevalue != null && dataTypePrevalue.ContainsKey(Constants.DefaultFieldsNames.DefaultDarkColorFieldName) ? dataTypePrevalue[Constants.DefaultFieldsNames.DefaultDarkColorFieldName] : null,
                LightColor = dataTypePrevalue != null && dataTypePrevalue.ContainsKey(Constants.DefaultFieldsNames.DefaultLightColorFieldName) ? dataTypePrevalue[Constants.DefaultFieldsNames.DefaultLightColorFieldName] : null,
                DrawQuiteZone = dataTypePrevalue != null && dataTypePrevalue.ContainsKey(Constants.DefaultFieldsNames.DefaultDrawQuietZoneFieldName) ? dataTypePrevalue[Constants.DefaultFieldsNames.DefaultDrawQuietZoneFieldName]?.StringToBoolean(false) : null,
                IconBorderWidth = dataTypePrevalue != null && dataTypePrevalue.ContainsKey(Constants.DefaultFieldsNames.DefaultIconBorderWidthFieldName) ? int.Parse(dataTypePrevalue[Constants.DefaultFieldsNames.DefaultIconBorderWidthFieldName]) : -1,
                Icon = dataTypePrevalue != null && dataTypePrevalue.ContainsKey(Constants.DefaultFieldsNames.DefaultIconFieldName) ? dataTypePrevalue[Constants.DefaultFieldsNames.DefaultIconFieldName] : null,
                ECCLevel = dataTypePrevalue != null && dataTypePrevalue.ContainsKey(Constants.DefaultFieldsNames.DefaultECCLevelFieldName) ? (ECCLevel)Enum.Parse(typeof(ECCLevel), dataTypePrevalue[Constants.DefaultFieldsNames.DefaultECCLevelFieldName]) : null
            };

            CompleteSettings(ref settings);

            return settings;
        }

        private static IDictionary<string, string> GetDataTypePrevalues(IPublishedContent publishedContent, string propertyAlias)
        {
            var dataType = publishedContent.GetProperty(propertyAlias)?.PropertyType.DataType;
            if (dataType?.EditorAlias == Constants.Backoffice.PropertyEditorAlias && dataType.Configuration != null)
            {
                var configuration = ((IEnumerable<KeyValuePair<string, object>>)dataType.Configuration)?.ToDictionary(k => k.Key, v => v.Value?.ToString());

                return configuration;
            }
            return null;
        }

        private static QRCodeSettings MargeSettings(QRCodeSettings defaultSettings, QRCodeSettings userSettings)
        {
            if (defaultSettings is null)
            {
                throw new ArgumentNullException(nameof(defaultSettings));
            }

            var settings = (QRCodeSettings)defaultSettings?.Clone();

            if (userSettings is not null)
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
