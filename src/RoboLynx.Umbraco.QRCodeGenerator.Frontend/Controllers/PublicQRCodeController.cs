using RoboLynx.Umbraco.QRCodeGenerator.Exceptions;
using RoboLynx.Umbraco.QRCodeGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Umbraco.Core;
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

        public PublicQRCodeController(IQRCodeBuilder qrCodeBuilder)
        {
            _qrCodeBuilder = qrCodeBuilder;
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
