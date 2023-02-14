using RoboLynx.Umbraco.QRCodeGenerator.Cache;
using RoboLynx.Umbraco.QRCodeGenerator.Extensions;
using RoboLynx.Umbraco.QRCodeGenerator.Models;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            return _formats.FirstOrDefault(f => f.Id == settings.Format)?.Create(codeType, settings) ?? throw new ArgumentException($"{FieldsNames.FormatFieldName} parameter has wrong value.");
        }

        private IQRCodeSource GetSource(string id, IPublishedContent publishedContent, string culture, string sourceSettings)
        {
            return _sources.FirstOrDefault(f => f.Id == id)?.Create(publishedContent, sourceSettings, culture) ?? throw new ArgumentException($"{FieldsNames.FormatFieldName} parameter has wrong value.");
        }

        private IQRCodeType GetType(string id, IQRCodeSource qtCodeSource)
        {
            return _types.FirstOrDefault(f => f.Id == id)?.Create(qtCodeSource) ?? throw new ArgumentException($"{FieldsNames.FormatFieldName} parameter has wrong value.");
        }

        public QRCodeSettings? GetDefaultSettings(IPublishedContent publishedContent, string propertyAlias)
        {
            var prevalues = GetDataTypePrevalues(publishedContent, propertyAlias);
            if (prevalues is null)
            {
                return null;
            }
            return CreateDefaultSettings(prevalues);
        }

        public QRCodeConfig CreateConfiguration(IPublishedContent publishedContent, string propertyAlias, string? culture, QRCodeSettings? userSettings)
        {
            if (publishedContent is null)
            {
                throw new ArgumentNullException(nameof(publishedContent));
            }
            var dataTypePrevalue = GetDataTypePrevalues(publishedContent, propertyAlias);

            if (dataTypePrevalue is not null)
            {
                var finalSettings = MargeSettings(CreateDefaultSettings(dataTypePrevalue), userSettings);
                var codeSourceSettings = dataTypePrevalue != null && dataTypePrevalue.ContainsKey(FieldsNames.CodeSourceSettingsFieldName) ? dataTypePrevalue[FieldsNames.CodeSourceSettingsFieldName] : string.Empty;

#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8604 // Possible null reference argument.
                var codeSource = dataTypePrevalue.ContainsKey(FieldsNames.CodeSourceFieldName) ? ((GetSource(dataTypePrevalue[FieldsNames.CodeSourceFieldName], publishedContent, culture, codeSourceSettings)) ?? throw new ArgumentException($"{FieldsNames.CodeSourceFieldName} parameter has wrong value.")) : throw new ArgumentException($"{FieldsNames.CodeSourceFieldName} parameter is not set up.");
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8604 // Possible null reference argument.
                var codeType = dataTypePrevalue.ContainsKey(FieldsNames.CodeTypeFieldName) ? ((GetType(dataTypePrevalue[FieldsNames.CodeTypeFieldName], codeSource)) ?? throw new ArgumentException($"{FieldsNames.CodeTypeFieldName} parameter has wrong value.")) : throw new ArgumentException($"{FieldsNames.CodeTypeFieldName} parameter is not set up.");
#pragma warning restore CS8604 // Possible null reference argument.

                var codeFormat = GetFormat(codeType, finalSettings);

                CompleteSettings(ref finalSettings);

                return new QRCodeConfig(codeSource, codeType, codeFormat, finalSettings);
            }
            throw new MissingFieldException("Property not exist or has a wrong property editor type.");
        }

        public QRCodeConfig CreateConfiguration(IQRCodeType codeType, QRCodeSettings? userSettings)
        {
            if (codeType is null)
            {
                throw new ArgumentNullException(nameof(codeType));
            }

            if (userSettings == null)
            {
                userSettings = new QRCodeSettings();
            }
            CompleteSettings(ref userSettings);

            var codeFormat = GetFormat(codeType, userSettings);

            return new QRCodeConfig(codeType, codeFormat, userSettings);
        }

        private static void CompleteSettings(ref QRCodeSettings userSettings)
        {
            if (string.IsNullOrEmpty(userSettings.DarkColor))
                userSettings.DarkColor = DefaultFieldsValues.DefaultDarkColorFieldValue;

            if (string.IsNullOrEmpty(userSettings.LightColor))
                userSettings.DarkColor = DefaultFieldsValues.DefaultLightColorFieldValue;

            if (!userSettings.Size.HasValue || userSettings.Size <= 0)
                userSettings.Size = DefaultFieldsValues.DefaultSizeFieldValue;

            if (!userSettings.ECCLevel.HasValue)
                userSettings.ECCLevel = DefaultFieldsValues.DefaultECCLevelFieldValue;

            if (!userSettings.DrawQuiteZone.HasValue)
                userSettings.DrawQuiteZone = DefaultFieldsValues.DefaultDrawQuietZoneFieldValue;

            if (!userSettings.IconSizePercent.HasValue || userSettings.IconSizePercent <= 0)
                userSettings.IconSizePercent = DefaultFieldsValues.DefaultIconSizePercentFieldValue;

            if (!userSettings.IconBorderWidth.HasValue || userSettings.IconBorderWidth < 0)
                userSettings.IconBorderWidth = DefaultFieldsValues.DefaultIconBorderWidthFieldValue;

            if (string.IsNullOrEmpty(userSettings.Format))
                userSettings.Format = DefaultFieldsValues.DefaultFormatFieldValue;
        }

        public Stream CreateStream(QRCodeConfig config, string? cacheName)
        {
            var hashId = config.Format.HashId;
            var extension = config.Format.FileExtension;

            if (CacheManager.IsCached(hashId, cacheName))
            {
                var stream = CacheManager.GetStream(hashId, cacheName);

                if (stream is not null)
                {
                    return stream;
                }
            }

            var qrCodeStream = config.Format.Stream();
            qrCodeStream.Seek(0, SeekOrigin.Begin);

            CacheManager.Add(hashId, extension, qrCodeStream, cacheName);

            qrCodeStream.Seek(0, SeekOrigin.Begin);
            return qrCodeStream;
        }

        private static QRCodeSettings CreateDefaultSettings(IDictionary<string, string?> dataTypePrevalue)
        {
            var settings = new QRCodeSettings();
            if (dataTypePrevalue is not null)
            {
                if (dataTypePrevalue.ContainsKey(DefaultFieldsNames.DefaultSizeFieldName))
                {
                    if (int.TryParse(dataTypePrevalue[DefaultFieldsNames.DefaultSizeFieldName], out var setSizeFieldValue))
                    {
                        settings.Size = setSizeFieldValue;
                    }
                }

                if (dataTypePrevalue.ContainsKey(DefaultFieldsNames.DefaultIconSizePercentFieldName)
                    && int.TryParse(dataTypePrevalue[DefaultFieldsNames.DefaultIconSizePercentFieldName], out var setIconSizePercentFieldValue))
                {
                    settings.IconSizePercent = setIconSizePercentFieldValue;
                }

                if (dataTypePrevalue.ContainsKey(DefaultFieldsNames.DefaultFormatFieldName))
                {
                    settings.Format = dataTypePrevalue[DefaultFieldsNames.DefaultFormatFieldName];
                }

                if (dataTypePrevalue.ContainsKey(DefaultFieldsNames.DefaultDarkColorFieldName))
                {
                    settings.DarkColor = dataTypePrevalue[DefaultFieldsNames.DefaultDarkColorFieldName];
                }

                if (dataTypePrevalue.ContainsKey(DefaultFieldsNames.DefaultLightColorFieldName))
                {
                    settings.LightColor = dataTypePrevalue[DefaultFieldsNames.DefaultLightColorFieldName];
                }

                if (dataTypePrevalue.ContainsKey(DefaultFieldsNames.DefaultDrawQuietZoneFieldName))
                {
                    settings.DrawQuiteZone = dataTypePrevalue[DefaultFieldsNames.DefaultDrawQuietZoneFieldName]?.StringToBoolean(false);
                }

                if (dataTypePrevalue.ContainsKey(DefaultFieldsNames.DefaultIconBorderWidthFieldName)
                    && int.TryParse(dataTypePrevalue[DefaultFieldsNames.DefaultIconBorderWidthFieldName], out var setIconBorderWidthFieldValue))
                {
                    settings.IconBorderWidth = setIconBorderWidthFieldValue;
                }

                if (dataTypePrevalue.ContainsKey(DefaultFieldsNames.DefaultIconFieldName))
                {
                    settings.Icon = dataTypePrevalue[DefaultFieldsNames.DefaultIconFieldName];
                }

                if (dataTypePrevalue.ContainsKey(DefaultFieldsNames.DefaultECCLevelFieldName)
                    && Enum.TryParse<ECCLevel>(dataTypePrevalue[DefaultFieldsNames.DefaultECCLevelFieldName], out var setECCLevelFieldValue))
                {
                    settings.ECCLevel = setECCLevelFieldValue;
                }
            }

            CompleteSettings(ref settings);

            return settings;
        }

        private static IDictionary<string, string?>? GetDataTypePrevalues(IPublishedContent publishedContent, string propertyAlias)
        {
            var dataType = publishedContent.GetProperty(propertyAlias)?.PropertyType.DataType;
            if (dataType?.EditorAlias == Backoffice.PropertyEditorAlias && dataType.Configuration != null)
            {
                var configuration = ((IEnumerable<KeyValuePair<string, object>>)dataType.Configuration)?.ToDictionary(k => k.Key, v => v.Value?.ToString());

                return configuration;
            }
            return null;
        }

        private static QRCodeSettings MargeSettings(QRCodeSettings defaultSettings, QRCodeSettings? userSettings)
        {
            if (defaultSettings is null)
            {
                throw new ArgumentNullException(nameof(defaultSettings));
            }

            var settings = (QRCodeSettings)defaultSettings.Clone();

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