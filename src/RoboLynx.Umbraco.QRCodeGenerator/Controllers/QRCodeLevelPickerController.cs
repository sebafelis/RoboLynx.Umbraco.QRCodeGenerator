using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco.Cms.Web.Common.Attributes;
using Umbraco.Cms.Web.Common.Filters;
using Umbraco.Extensions;

namespace RoboLynx.Umbraco.QRCodeGenerator.Controllers
{
    [PluginController(Constants.Core.PluginAlias)]
    [AngularJsonOnlyConfiguration]
    public class QRCodeLevelPickerController : UmbracoAuthorizedJsonController
    {
        private readonly ILocalizedTextService _localizedTextService;

        public QRCodeLevelPickerController(ILocalizedTextService localizedTextService)
        {
            this._localizedTextService = localizedTextService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var eccLevelValues = Enum.GetValues(typeof(ECCLevel)).Cast<ECCLevel>();
            var result = eccLevelValues.Select(l => new { id = l.ToString(), name = _localizedTextService.Localize("qrCodeLevels", $"{l}Name"), description = _localizedTextService.Localize("qrCodeLevels", $"{l}Description") });

            return Ok(result);
        }
    }
}