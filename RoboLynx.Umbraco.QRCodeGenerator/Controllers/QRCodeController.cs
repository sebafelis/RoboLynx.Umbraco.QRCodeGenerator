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
using System.Linq;
using RoboLynx.Umbraco.QRCodeGenerator.Exceptions;
using Umbraco.Core.Services;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources;
using Umbraco.Core.Models.PublishedContent;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes;
using RoboLynx.Umbraco.QRCodeGenerator.Extensions;
using Umbraco.Core.Persistence;
using Umbraco.Web;
using Umbraco.Core.Configuration;
using Umbraco.Core.Logging;
using Umbraco.Core.Cache;
using Umbraco.Core.Mapping;
using Umbraco.Web.Composing;
using RoboLynx.Umbraco.QRCodeGenerator.Models;

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
        public IHttpActionResult DefaultSettings(int contentId, string propertyAlias)
        {
            var publishedContent = Umbraco.Content(contentId);

            if (publishedContent is null)
            {
                return BadRequest();
            }

            var defaultSettings = qrCodeConfigBuilder.GetDefaultSettings(publishedContent, propertyAlias);

            if (defaultSettings is null)
            {
                return NotFound();
            }

            return Ok(defaultSettings);
        }

        [HttpGet]
        public IHttpActionResult RequiredSettingsForFormats()
        {
            var requierdSettingsForFormats = formats.ToDictionary(k => k.Id, v => v.RequiredSettings);

            return Ok(requierdSettingsForFormats);
        }

        [HttpGet]
        //[CompressContent]
        public IHttpActionResult Image(int contentId, string propertyAlias, [FromUri] QRCodeSettings settings)
        {
            var publishedContent = Umbraco.Content(contentId);

            if (publishedContent != null)
            {
                try
                {
                    var response = Request.CreateResponse(HttpStatusCode.OK);

                    response.Content = qrCodeConfigBuilder.CreateQRCodeAsResponse(publishedContent, propertyAlias, settings);

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
            return BadRequest();
        }
    }
}