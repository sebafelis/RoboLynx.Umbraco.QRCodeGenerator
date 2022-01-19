using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RoboLynx.Umbraco.QRCodeGenerator.Exceptions;
using RoboLynx.Umbraco.QRCodeGenerator.Models;
using System;
using System.Threading.Tasks;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Web.Common;
using Umbraco.Cms.Web.Common.Controllers;
using Umbraco.Cms.Web.Common.Attributes;

namespace RoboLynx.Umbraco.QRCodeGenerator.Frontend.Controllers
{
    [PluginController(Constants.Core.PluginAlias)]
    public class PublicQRCodeController : UmbracoApiController
    {
        private readonly IQRCodeResponesFactory _responesFactory;
        private readonly ILogger<PublicQRCodeController> _logger;
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IMemberManager _memberManager;
        private readonly IEntityService _entityService;

        public PublicQRCodeController(IQRCodeResponesFactory responesFactory, ILogger<PublicQRCodeController> logger, UmbracoHelper umbracoHelper, IMemberManager memberManager, IEntityService entityService)
        {
            _responesFactory = responesFactory;
            _logger = logger;
            _umbracoHelper = umbracoHelper;
            _memberManager = memberManager;
            _entityService = entityService;
        }

        [HttpGet]
        //[CompressContent]
        public async Task<IActionResult> Get(Udi nodeUdi, string propertyAlias, [FromQuery] QRCodeSettings settings, string culture = null)
        {
            if (nodeUdi is null || propertyAlias is null)
            {
                return BadRequest();
            }
                
            IPublishedContent publishedContent = await GetPublishedContent(nodeUdi);

            return _responesFactory.CreateResponesWithQRCode(publishedContent, propertyAlias, culture, settings, Constants.Frontend.FrontendCacheName);
        }

        private async Task<IPublishedContent> GetPublishedContent(Udi nodeUdi)
        {
            var umbracoType = ObjectTypes.GetUmbracoObjectType(nodeUdi.EntityType);
            IPublishedContent publishedContent = null;

            switch (umbracoType)
            {
                case UmbracoObjectTypes.Document:
                    publishedContent = _umbracoHelper.Content(nodeUdi);
                    break;
                case UmbracoObjectTypes.Media:
                    publishedContent = _umbracoHelper.Media(nodeUdi);
                    break;
                case UmbracoObjectTypes.Member:
                    var nodeGuid = (nodeUdi as GuidUdi).Guid;
                    var member = _entityService.Get(nodeGuid, UmbracoObjectTypes.Member);
                    var name = member.Name;
                    var user = await _memberManager.FindByIdAsync(name);
                    if (user != null)
                    {
                        publishedContent = _memberManager.AsPublishedMember(user);
                    }
                    break;
            }

            if (publishedContent == null)
            {
                _logger.LogWarning("Published content not found.");
            }

            return publishedContent;
        }
    }
}
