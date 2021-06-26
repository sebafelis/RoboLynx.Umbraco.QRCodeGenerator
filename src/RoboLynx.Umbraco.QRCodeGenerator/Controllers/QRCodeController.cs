using RoboLynx.Umbraco.QRCodeGenerator.Exceptions;
using RoboLynx.Umbraco.QRCodeGenerator.Models;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Cache;
using Umbraco.Core.Configuration;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.Persistence;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;
using Umbraco.Web.PublishedCache;
using Umbraco.Web.WebApi;
using System.Net.Http.Headers;
using Umbraco.Core.Composing;

namespace RoboLynx.Umbraco.QRCodeGenerator.Controllers
{
    [PluginController(Constants.PluginAlias)]
    [JsonCamelCaseFormatter]
    public class QRCodeController : UmbracoAuthorizedJsonController
    {
        private readonly IQRCodeBuilder _qrCodeBuilder;
        private readonly QRCodeFormatFactoryCollection _formats;

        public QRCodeController(IQRCodeBuilder qrCodeBuilder, QRCodeFormatFactoryCollection formats,
                                IGlobalSettings globalSettings, IUmbracoContextAccessor umbracoContextAccessor,
                                ISqlContext sqlContext, ServiceContext services, AppCaches appCaches,
                                IProfilingLogger logger, IRuntimeState runtimeState, UmbracoHelper umbracoHelper) : base(globalSettings,
                                    umbracoContextAccessor, sqlContext, services, appCaches, logger, runtimeState,
                                    umbracoHelper)
        {
            _qrCodeBuilder = qrCodeBuilder;
            _formats = formats ?? throw new ArgumentNullException(nameof(formats));
        }

        [HttpGet]
        public IHttpActionResult DefaultSettings(int nodeId, string propertyAlias)
        {
            var objectType = Services.EntityService.GetObjectType(nodeId);
            var nodeKey = Services.EntityService.GetKey(nodeId, objectType);
            if (!nodeKey.Success)
            {
                return NotFound();
            }
            var nodeUdi = Udi.Create(objectType.GetUdiType(), nodeKey.Result);

            return DefaultSettings(nodeUdi, propertyAlias);
        }

        [HttpGet]
        public IHttpActionResult DefaultSettings(Udi nodeUdi, string propertyAlias)
        {
            var publishedContent = Umbraco.PublishedContent(nodeUdi);

            if (publishedContent != null)
            {
                try
                {
                    var defaultSettings = _qrCodeBuilder.GetDefaultSettings(publishedContent, propertyAlias);

                    if (defaultSettings is null)
                    {
                        return BadRequest("Content has not configuration.");
                    }

                    return Ok(defaultSettings);
                }
                catch (QRCodeGeneratorException qrex)
                {
                    return BadRequest(qrex.Message);
                }
                catch (Exception ex) when (ex is ArgumentException || ex is ArgumentNullException)
                {
                    return BadRequest();
                }
            }

            return NotFound();
        }

        [HttpGet]
        public IHttpActionResult RequiredSettingsForFormats()
        {
            var requierdSettingsForFormats = _formats.ToDictionary(k => k.Id, v => v.RequiredSettings);

            return Ok(requierdSettingsForFormats);
        }

        [HttpGet]
        //[CompressContent]
        public IHttpActionResult Image(int nodeId, string propertyAlias, [FromUri] QRCodeSettings settings, string culture = null)
        {
            var objectType = Services.EntityService.GetObjectType(nodeId);
            var nodeKey = Services.EntityService.GetKey(nodeId, objectType);
            if (!nodeKey.Success)
            {
                return NotFound();
            }
            var nodeUdi = Udi.Create(objectType.GetUdiType(), nodeKey.Result);

            return Image(nodeUdi, propertyAlias, settings, culture);
        }

        [HttpGet]
        //[CompressContent]
        public IHttpActionResult Image(Udi nodeUdi, string propertyAlias, [FromUri] QRCodeSettings settings, string culture = null)
        {
            var publishedContent = Umbraco.PublishedContent(nodeUdi);

            if (publishedContent != null)
            {
                try
                {
                    var config = _qrCodeBuilder.CreateConfiguration(publishedContent, propertyAlias, culture, settings);

                    if (config is null)
                    {
                        return BadRequest();
                    }

                    var response = _qrCodeBuilder.CreateResponse(Request, config, true, Constants.BackofficeCacheName);

                    return ResponseMessage(response);
                }
                catch (QRCodeGeneratorException qrex)
                {
                    return BadRequest(qrex.Message);
                }
                catch (Exception ex) when (ex is ArgumentException || ex is ArgumentNullException)
                {
                    return BadRequest();
                }
            }

            return NotFound();
        }
    }
}