using RoboLynx.Umbraco.QRCodeGenerator.Exceptions;
using RoboLynx.Umbraco.QRCodeGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;

namespace RoboLynx.Umbraco.QRCodeGenerator.Frontend.Controllers
{
    [PluginController(RoboLynx.Umbraco.QRCodeGenerator.Constants.PluginAlias)]
    public class PublicQRCodeController : UmbracoApiController
    {
        private readonly IQRCodeBuilder _qrCodeBuilder;
        private readonly ILogger _logger;

        public PublicQRCodeController(IQRCodeBuilder qrCodeBuilder, ILogger logger)
        {
            _qrCodeBuilder = qrCodeBuilder;
            _logger = logger;
        }

        [HttpGet]
        //[CompressContent]
        public IHttpActionResult Get(int nodeId, string propertyAlias, [FromUri] QRCodeSettings settings, string culture = null)
        {
            var objectType = Services.EntityService.GetObjectType(nodeId);
            var nodeKey = Services.EntityService.GetKey(nodeId, objectType);
            if (!nodeKey.Success)
            {
                return NotFound();
            }
            var nodeUdi = Udi.Create(objectType.GetUdiType(), nodeKey.Result);

            return Get(nodeUdi, propertyAlias, settings, culture);
        }

        [HttpGet]
        //[CompressContent]
        public IHttpActionResult Get(Guid nodeKey, string propertyAlias, [FromUri] QRCodeSettings settings, string culture = null)
        {
            var objectType = Services.EntityService.GetObjectType(nodeKey);
            var nodeUdi = Udi.Create(objectType.GetUdiType(), nodeKey);

            return Get(nodeUdi, propertyAlias, settings, culture);
        }

        //[HttpGet]
        //[CompressContent]
        private IHttpActionResult Get(Udi nodeUdi, string propertyAlias, [FromUri] QRCodeSettings settings, string culture = null)
        {
            var publishedContent = Umbraco.PublishedContent(nodeUdi);

            if (publishedContent != null)
            {
                try
                {
                    var config = _qrCodeBuilder.CreateConfiguration(publishedContent, propertyAlias, culture, settings);

                    var response = _qrCodeBuilder.CreateResponse(Request, config, true, Constants.FrontendCacheName);

                    return ResponseMessage(response);
                }
                catch (Exception ex) when (ex is ArgumentException || ex is ArgumentNullException || ex is QRCodeGeneratorException)
                {
                    _logger.Error<PublicQRCodeController>("Occur an exception during QR code generation.", ex);
                    return BadRequest();
                }
            }

            return NotFound();
        }
    }
}
