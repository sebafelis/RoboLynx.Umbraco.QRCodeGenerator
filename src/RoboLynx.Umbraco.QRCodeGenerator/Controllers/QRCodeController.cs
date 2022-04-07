using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RoboLynx.Umbraco.QRCodeGenerator.Models;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat;
using System;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco.Cms.Web.BackOffice.Filters;
using Umbraco.Cms.Web.Common;
using Umbraco.Cms.Web.Common.Attributes;

namespace RoboLynx.Umbraco.QRCodeGenerator.Controllers
{
    [PluginController(Constants.Core.PluginAlias)]
    [JsonCamelCaseFormatter]
    public class QRCodeController : UmbracoAuthorizedJsonController
    {
        private readonly IQRCodeResponesFactory _responesFactory;
        private readonly QRCodeFormatFactoryCollection _formats;
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IMemberManager _memberManager;
        private readonly IIdKeyMap _idKeyMap;
        private readonly ILogger<QRCodeController> _logger;

        public QRCodeController(IQRCodeResponesFactory responesFactory, QRCodeFormatFactoryCollection formats, UmbracoHelper umbracoHelper,
            IMemberManager memberManager, IIdKeyMap idKeyMap, ILogger<QRCodeController> logger)
        {
            _responesFactory = responesFactory;
            _formats = formats ?? throw new ArgumentNullException(nameof(formats));
            _umbracoHelper = umbracoHelper;
            _memberManager = memberManager;
            _idKeyMap = idKeyMap;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> DefaultSettings(Udi nodeUdi, string propertyAlias)
        {
            IPublishedContent publishedContent = await GetPublishedContent(nodeUdi);

            return _responesFactory.CreateResponseWithDefaultSettings(publishedContent, propertyAlias);
        }

        [HttpGet]
        public IActionResult RequiredSettingsForFormats()
        {
            var requierdSettingsForFormats = _formats.ToDictionary(k => k.Id, v => v.RequiredSettings);

            return Ok(requierdSettingsForFormats);
        }

        [HttpGet]
        public async Task<IActionResult> Image(Udi nodeUdi, string propertyAlias, [FromQuery] QRCodeSettings settings, string culture = null)
        {
            IPublishedContent publishedContent = await GetPublishedContent(nodeUdi);

            return _responesFactory.CreateResponesWithQRCode(publishedContent, propertyAlias, culture, settings, Constants.Backoffice.BackofficeCacheName);
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