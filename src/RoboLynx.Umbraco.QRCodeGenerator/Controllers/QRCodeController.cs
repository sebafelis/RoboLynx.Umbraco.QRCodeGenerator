using RoboLynx.Umbraco.QRCodeGenerator.Exceptions;
using RoboLynx.Umbraco.QRCodeGenerator.Extensions;
using RoboLynx.Umbraco.QRCodeGenerator.Models;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Umbraco.Core;
using Umbraco.Core.Cache;
using Umbraco.Core.Configuration;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Core.Persistence;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;

namespace RoboLynx.Umbraco.QRCodeGenerator.Controllers
{
    [PluginController(Constants.PluginAlias)]
    [JsonCamelCaseFormatter]
    public class QRCodeController : UmbracoAuthorizedJsonController
    {
        private readonly IQRCodeBuilder qrCodeConfigBuilder;
        private readonly QRCodeFormatsCollection formats;


        public QRCodeController(IQRCodeBuilder qrCodeConfigBuilder, QRCodeFormatsCollection formats,
                                IGlobalSettings globalSettings, IUmbracoContextAccessor umbracoContextAccessor,
                                ISqlContext sqlContext, ServiceContext services, AppCaches appCaches,
                                IProfilingLogger logger, IRuntimeState runtimeState, UmbracoHelper umbracoHelper) : base(globalSettings,
                                    umbracoContextAccessor, sqlContext, services, appCaches, logger, runtimeState,
                                    umbracoHelper)
        {
            this.qrCodeConfigBuilder = qrCodeConfigBuilder ?? throw new ArgumentNullException(nameof(qrCodeConfigBuilder));
            this.formats = formats ?? throw new ArgumentNullException(nameof(formats));
        }

        [HttpGet]
        public IHttpActionResult DefaultSettings(int nodeId, string propertyAlias)
        {
            var objectType = Services.EntityService.GetObjectType(nodeId);

            switch (objectType)
            {
                case UmbracoObjectTypes.Document:
                    var publishedContent = Umbraco.Content(nodeId);

                    if (publishedContent is null)
                    {
                        return BadRequest("Content is not published or is not in cache yet.");
                    }

                    var defaultSettings = qrCodeConfigBuilder.GetDefaultSettings(publishedContent, propertyAlias);

                    if (defaultSettings is null)
                    {
                        return BadRequest("Content has not configuration.");
                    }

                    return Ok(defaultSettings);
                case UmbracoObjectTypes.Unknown:
                    return NotFound();
                default:
                    return BadRequest("This node type is not supported.");
            }
        }

        [HttpGet]
        public IHttpActionResult RequiredSettingsForFormats()
        {
            var requierdSettingsForFormats = formats.ToDictionary(k => k.Id, v => v.RequiredSettings);

            return Ok(requierdSettingsForFormats);
        }

        [HttpGet]
        //[CompressContent]
        public IHttpActionResult Image(int nodeId, string propertyAlias, [FromUri] QRCodeSettings settings, string culture = null)
        {
            var objectType = Services.EntityService.GetObjectType(nodeId);

            switch (objectType)
            {
                case UmbracoObjectTypes.Document:
                    var publishedContent = Umbraco.Content(nodeId);

                    if (publishedContent != null)
                    {
                        try
                        {
                            var response = Request.CreateResponse(HttpStatusCode.OK);

                            response.Content = qrCodeConfigBuilder.CreateQRCodeAsResponse(publishedContent, propertyAlias, culture, settings);

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
                    return BadRequest("Content is not published or is not in cache yet.");
                case UmbracoObjectTypes.Unknown:
                    return NotFound();
                default:
                    return BadRequest("This node type is not supported.");
            }
        }

    }
}