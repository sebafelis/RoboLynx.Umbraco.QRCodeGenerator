using RoboLynx.Umbraco.QRCodeGenerator.Exceptions;
using RoboLynx.Umbraco.QRCodeGenerator.Models;
using System;
using System.Web.Http;
using Umbraco.Core;
using Umbraco.Core.Logging;
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
        public IHttpActionResult Get(Udi nodeUdi, string propertyAlias, [FromUri] QRCodeSettings settings, string culture = null)
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
