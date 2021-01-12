using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Umbraco.Web.WebApi;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;
using Umbraco.Core;
using Umbraco.Core.Models;
using System.Collections.Generic;
using RoboLynx.Umbraco.QRCodeGenerator.Helpers;
using System.Web.Http.Results;
using System.Linq;
using System.Diagnostics;

namespace RoboLynx.Umbraco.QRCodeGenerator.Controllers
{
    [PluginController(RoboLynx.Umbraco.QRCodeGenerator.Constants.PluginName)]
    [JsonCamelCaseFormatter]
    public class QRCodeController : UmbracoAuthorizedJsonController
    {


        [HttpGet]
        public IHttpActionResult DefaultSettings(int contentId, string propertyAlias)
        {
            var defaultSettings = GetDefaultSettings(GetDataTypePrevalues(contentId, propertyAlias));

            return Ok(defaultSettings);
        }

        [HttpGet]
        public IHttpActionResult RequiredSettingsForFormats()
        {
            var requierdSettingsForFormats = QRCodeHelper.GetQRCodeFormatProviders().ToDictionary(k => k.Id, v => v.RequiredSettings);

            return Ok(requierdSettingsForFormats);
        }

        [HttpGet]
        public IHttpActionResult Image(int contentId, string propertyAlias, [FromUri] QRCodeSettings settings)
        {
            var finallSettings = GetSettings(contentId, propertyAlias, settings);

            var content = Umbraco.TypedContent(contentId);

            if (content != null)
            {
                var value = finallSettings.Type.Value;

                return CreateQRCodeResponse(value, finallSettings.DefaultSettings);

            }
            return StatusCode(HttpStatusCode.NotAcceptable);
        }

        private IHttpActionResult CreateQRCodeResponse(string value, QRCodeSettings settings)
        {
            if (!string.IsNullOrEmpty(value))
            {
                var response = Request.CreateResponse(HttpStatusCode.OK);

                var formatProvider = QRCodeHelper.GetQRCodeFormatProviderById(settings.Format, Constants.GetDefaultFormat());

                response.Content = formatProvider.ResponseContent(value, settings, Umbraco);

                return ResponseMessage(response);
            }
            return StatusCode(HttpStatusCode.NotAcceptable);
        }


        private QRCodeSettings MargeSettings(QRCodeSettings defaultSettings, QRCodeSettings userSettings)
        {
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

        private QRCodeSettings GetDefaultSettings(IDictionary<string, PreValue> dataTypePrevalue)
        {
            return new QRCodeSettings()
            {
                Size = dataTypePrevalue != null && dataTypePrevalue.ContainsKey(Constants.DefaultSizeFieldName) ? int.Parse(dataTypePrevalue[Constants.DefaultSizeFieldName].Value) : 20,
                IconSizePercent = dataTypePrevalue != null && dataTypePrevalue.ContainsKey(Constants.DefaultIconSizePercentFieldName) ? int.Parse(dataTypePrevalue[Constants.DefaultIconSizePercentFieldName].Value) : 5,
                Format = dataTypePrevalue != null && dataTypePrevalue.ContainsKey(Constants.DefaultFormatFieldName) ? dataTypePrevalue[Constants.DefaultFormatFieldName].Value : Constants.GetDefaultFormat().Id,
                DarkColor = dataTypePrevalue != null && dataTypePrevalue.ContainsKey(Constants.DefaultDarkColorFieldName) ? dataTypePrevalue[Constants.DefaultDarkColorFieldName].Value : "#000000",
                LightColor = dataTypePrevalue != null && dataTypePrevalue.ContainsKey(Constants.DefaultLightColorFieldName) ? dataTypePrevalue[Constants.DefaultLightColorFieldName].Value : "#FFFFFF",
                DrawQuiteZone = dataTypePrevalue != null && dataTypePrevalue.ContainsKey(Constants.DefaultDrawQuietZoneFieldName) ? QRCodeHelper.StringToBoolean(dataTypePrevalue[Constants.DefaultDrawQuietZoneFieldName].Value, false) : false,
                IconBorderWidth = dataTypePrevalue != null && dataTypePrevalue.ContainsKey(Constants.DefaultIconBorderWidthFieldName) ? int.Parse(dataTypePrevalue[Constants.DefaultIconBorderWidthFieldName].Value) : 2,
                Icon = dataTypePrevalue != null && dataTypePrevalue.ContainsKey(Constants.DefaultIconFieldName) ? dataTypePrevalue[Constants.DefaultIconFieldName].Value : null
            };
        }

        private QRCodePrevalues GetSettings(int contentId, string propertyAlias, QRCodeSettings userSettings)
        {
            var dataTypePrevalue = GetDataTypePrevalues(contentId, propertyAlias);

            var publishedContent = Umbraco.TypedContent(contentId);

            if (dataTypePrevalue != null)
            {
                var codeSourceSettings = dataTypePrevalue.ContainsKey(Constants.CodeSourceSettingsFieldName) ? dataTypePrevalue[Constants.CodeSourceSettingsFieldName].Value : string.Empty;
                var codeSource = dataTypePrevalue.ContainsKey(Constants.CodeSourceFieldName) ? QRCodeHelper.GetQRCodeSourceProviderByName(dataTypePrevalue[Constants.CodeSourceFieldName].Value, publishedContent, codeSourceSettings) : throw new ArgumentException($"{Constants.CodeSourceFieldName} parameter is not set up.");
                var codeType = dataTypePrevalue.ContainsKey(Constants.CodeTypeFieldName) ? QRCodeHelper.GetQRCodeTypeProviderByName(dataTypePrevalue[Constants.CodeTypeFieldName].Value, codeSource) : throw new ArgumentException($"{Constants.CodeTypeFieldName} parameter is not set up.");
                var finalSettings = MargeSettings(GetDefaultSettings(dataTypePrevalue), userSettings);

                return new QRCodePrevalues()
                {
                    DefaultSettings = finalSettings,
                    Source = codeSource,
                    Type = codeType
                };
            }
            return null;
        }

        private IDictionary<string, PreValue> GetDataTypePrevalues(int contentId, string propertyAlias)
        {
            var dataTypeDefinitionId = Services.ContentService.GetById(contentId)?.Properties[propertyAlias]?.PropertyType.DataTypeDefinitionId;
            if (dataTypeDefinitionId.HasValue)
            {
                return Services.DataTypeService.GetPreValuesCollectionByDataTypeId(dataTypeDefinitionId.Value)?.PreValuesAsDictionary;
            }
            return null;
        }
    }
}