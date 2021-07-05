using LightInject;
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
using System.Web;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.Scoping;
using Umbraco.Core.Services;

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
            return _formats.FirstOrDefault(f => f.Id == settings.Format).Create(codeType, settings) ?? throw new ArgumentException($"{Constants.FormatFieldName} parameter has wrong value.");
        }

        private IQRCodeSource GetSource(string id, IPublishedContent publishedContent, string culture, string sourceSettings)
        {
            return _sources.FirstOrDefault(f => f.Id == id).Create(publishedContent, sourceSettings, culture) ?? throw new ArgumentException($"{Constants.FormatFieldName} parameter has wrong value.");
        }

        private IQRCodeType GetType(string id, IQRCodeSource qtCodeSource)
        {
            return _types.FirstOrDefault(f => f.Id == id).Create(qtCodeSource) ?? throw new ArgumentException($"{Constants.FormatFieldName} parameter has wrong value.");
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
                var codeSourceSettings = dataTypePrevalue.ContainsKey(Constants.CodeSourceSettingsFieldName) ? dataTypePrevalue[Constants.CodeSourceSettingsFieldName] : string.Empty;
                var codeSource = dataTypePrevalue.ContainsKey(Constants.CodeSourceFieldName) ? ((GetSource(dataTypePrevalue[Constants.CodeSourceFieldName], publishedContent, culture, codeSourceSettings)) ?? throw new ArgumentException($"{Constants.CodeSourceFieldName} parameter has wrong value.")) : throw new ArgumentException($"{Constants.CodeSourceFieldName} parameter is not set up.");
                var codeType = dataTypePrevalue.ContainsKey(Constants.CodeTypeFieldName) ? ((GetType(dataTypePrevalue[Constants.CodeTypeFieldName], codeSource)) ?? throw new ArgumentException($"{Constants.CodeTypeFieldName} parameter has wrong value.")) : throw new ArgumentException($"{Constants.CodeTypeFieldName} parameter is not set up.");
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

        private void CompleteSettings(ref QRCodeSettings userSettings)
        {
            if (string.IsNullOrEmpty(userSettings.DarkColor))
                userSettings.DarkColor = Constants.DefaultDarkColorFieldValue;

            if (string.IsNullOrEmpty(userSettings.LightColor))
                userSettings.DarkColor = Constants.DefaultLightColorFieldValue;

            if (userSettings.Size <= 0)
                userSettings.Size = Constants.DefaultSizeFieldValue;

            if (!userSettings.ECCLevel.HasValue)
                userSettings.ECCLevel = Constants.DefaultECCLevelFieldValue;

            if (!userSettings.DrawQuiteZone.HasValue)
                userSettings.DrawQuiteZone = Constants.DefaultDrawQuietZoneFieldValue;

            if (userSettings.IconSizePercent <= 0)
                userSettings.IconSizePercent = Constants.DefaultIconSizePercentFieldValue;

            if (userSettings.IconBorderWidth < 0)
                userSettings.IconBorderWidth = Constants.DefaultIconBorderWidthFieldValue;

            if (string.IsNullOrEmpty(userSettings.Format))
                userSettings.Format = Constants.DefaultFormatFieldValue;
        }

        public HttpResponseMessage CreateResponse(HttpRequestMessage request, QRCodeConfig config, bool attachment = false, string cacheName = null)
        {
            HttpResponseMessage response;
            var hashId = config.Format.HashId;

            if (!CacheManager.UrlSupport(cacheName))
            {
                response = request.CreateResponse(System.Net.HttpStatusCode.OK);

                var stream = CreateStream(config, cacheName);

                var httpContent = new StreamContent(stream);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue(config.Format.Mime);
                httpContent.Headers.Expires = CacheManager.Expired(hashId, cacheName);

                if (attachment)
                {
                    httpContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                    var fileName = config.Format.FileName;
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        httpContent.Headers.ContentDisposition.FileName = fileName;
                    };
                }
                else
                {
                    httpContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("inline");
                }

                response.Content = httpContent;
            }
            else
            {
                response = request.CreateResponse(System.Net.HttpStatusCode.Redirect);                

                if (!CacheManager.IsCached(hashId, cacheName))
                {
                    _ = CreateStream(config, cacheName);
                }
                var cacheUrl = CacheManager.GetUrl(hashId, UrlMode.Default, cacheName);
                
                response.Headers.Location = new Uri(cacheUrl);
                response.Headers.CacheControl = new CacheControlHeaderValue() { NoStore = true };
            }

            return response;
        }

        public Stream CreateStream(QRCodeConfig config, string cacheName)
        {
            var hashId = config.Format.HashId;
            var extension = config.Format.FileExtension;

            if (CacheManager.IsCached(hashId, cacheName))
            {
                return CacheManager.GetStream(hashId, cacheName);
            }
            else
            {
                var qrCodeStream = config.Format.Stream();

                CacheManager.Add(hashId, extension, qrCodeStream, cacheName);

                return qrCodeStream;
            }
        }

        public string GetUrl(QRCodeConfig config, string cacheName)
        {
            var hashId = config.Format.HashId;

            if (CacheManager.UrlSupport(cacheName))
            {
                if (!CacheManager.IsCached(hashId, cacheName))
                {
                    _ = CreateStream(config, cacheName);
                }
                return CacheManager.GetUrl(hashId, UrlMode.Default, cacheName);
            }
            throw new NotSupportedException("Configuration not allow URL direct to cache.");
        }

        private QRCodeSettings CreateDefaultSettings(IDictionary<string, string> dataTypePrevalue)
        {
            if (dataTypePrevalue is null)
            {
                return null;
            }

            var settings = new QRCodeSettings()
            {
                Size = dataTypePrevalue != null && dataTypePrevalue.ContainsKey(Constants.DefaultSizeFieldName) ? int.Parse(dataTypePrevalue[Constants.DefaultSizeFieldName]) : 0,
                IconSizePercent = dataTypePrevalue != null && dataTypePrevalue.ContainsKey(Constants.DefaultIconSizePercentFieldName) ? int.Parse(dataTypePrevalue[Constants.DefaultIconSizePercentFieldName]) : 0,
                Format = dataTypePrevalue != null && dataTypePrevalue.ContainsKey(Constants.DefaultFormatFieldName) ? dataTypePrevalue[Constants.DefaultFormatFieldName] : null,
                DarkColor = dataTypePrevalue != null && dataTypePrevalue.ContainsKey(Constants.DefaultDarkColorFieldName) ? dataTypePrevalue[Constants.DefaultDarkColorFieldName] : null,
                LightColor = dataTypePrevalue != null && dataTypePrevalue.ContainsKey(Constants.DefaultLightColorFieldName) ? dataTypePrevalue[Constants.DefaultLightColorFieldName] : null,
                DrawQuiteZone = dataTypePrevalue != null && dataTypePrevalue.ContainsKey(Constants.DefaultDrawQuietZoneFieldName) ? dataTypePrevalue[Constants.DefaultDrawQuietZoneFieldName]?.StringToBoolean(false) : null,
                IconBorderWidth = dataTypePrevalue != null && dataTypePrevalue.ContainsKey(Constants.DefaultIconBorderWidthFieldName) ? int.Parse(dataTypePrevalue[Constants.DefaultIconBorderWidthFieldName]) : -1,
                Icon = dataTypePrevalue != null && dataTypePrevalue.ContainsKey(Constants.DefaultIconFieldName) ? dataTypePrevalue[Constants.DefaultIconFieldName] : null,
                ECCLevel = dataTypePrevalue != null && dataTypePrevalue.ContainsKey(Constants.DefaultECCLevelFieldName) ? (ECCLevel)Enum.Parse(typeof(ECCLevel), dataTypePrevalue[Constants.DefaultECCLevelFieldName]) : null
            };

            CompleteSettings(ref settings);

            return settings;
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
