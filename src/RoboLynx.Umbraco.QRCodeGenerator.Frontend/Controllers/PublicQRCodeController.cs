using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RoboLynx.Umbraco.QRCodeGenerator.Models;
using System.Threading.Tasks;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Web.Common;
using Umbraco.Cms.Web.Common.Attributes;
using Umbraco.Cms.Web.Common.Controllers;

namespace RoboLynx.Umbraco.QRCodeGenerator.Frontend.Controllers
{
    [PluginController(Core.PluginAlias)]
    public class PublicQRCodeController : UmbracoApiController
    {
        private readonly IQRCodeResponesFactory _responesFactory;
        private readonly ILogger<PublicQRCodeController> _logger;
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IMemberManager _memberManager;
        private readonly IIdKeyMap _idKeyMap;

        public PublicQRCodeController(IQRCodeResponesFactory responesFactory, ILogger<PublicQRCodeController> logger,
            UmbracoHelper umbracoHelper, IMemberManager memberManager, IIdKeyMap idKeyMap)
        {
            _responesFactory = responesFactory;
            _logger = logger;
            _umbracoHelper = umbracoHelper;
            _memberManager = memberManager;
            _idKeyMap = idKeyMap;
        }

        [HttpGet]
        [TypeFilter(typeof(DecryptQueryParametersAttribute))]
        public async Task<IActionResult> Get(Udi nodeUdi, string propertyAlias, [FromQuery] QRCodeSettings? settings, string? culture = null)
        {
            if (nodeUdi is null || propertyAlias is null)
            {
                return BadRequest();
            }

            IPublishedContent? publishedContent = await GetPublishedContent(nodeUdi);

            if (publishedContent is null)
            {
                return BadRequest();
            }

            return _responesFactory.CreateResponesWithQRCode(publishedContent, propertyAlias, culture, settings, Frontend.FrontendCacheName);
        }

        private async Task<IPublishedContent?> GetPublishedContent(Udi nodeUdi)
        {
            var umbracoType = ObjectTypes.GetUmbracoObjectType(nodeUdi.EntityType);
            IPublishedContent? publishedContent = null;

            switch (umbracoType)
            {
                case UmbracoObjectTypes.Document:
                    publishedContent = _umbracoHelper.Content(nodeUdi);
                    break;

                case UmbracoObjectTypes.Media:
                    publishedContent = _umbracoHelper.Media(nodeUdi);
                    break;

                case UmbracoObjectTypes.Member:
                    var memberAttempt = _idKeyMap.GetIdForUdi(nodeUdi);
                    if (memberAttempt.Success)
                    {
                        var memberId = memberAttempt.Result;
                        var member = await _memberManager.FindByIdAsync(memberId.ToString());
                        if (member != null)
                        {
                            publishedContent = _memberManager.AsPublishedMember(member);
                        }
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