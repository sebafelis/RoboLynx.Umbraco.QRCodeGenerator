using System.Web.Http;
using Umbraco.Web.Editors;
using System.Linq;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;
using Umbraco.Core;
using System;
using Umbraco.Core.Services;

namespace RoboLynx.Umbraco.QRCodeGenerator.Controllers
{
    [PluginController(RoboLynx.Umbraco.QRCodeGenerator.Constants.PluginAlias)]
    [AngularJsonOnlyConfiguration]
    public class QRCodeLevelPickerController : UmbracoAuthorizedJsonController
    {
        [HttpGet]
        public IHttpActionResult Get()
        {
            var eccLevelValues = Enum.GetValues(typeof(ECCLevel)).Cast<ECCLevel>();
            var result = eccLevelValues.Select(l => new { id = l.ToString(), name = ApplicationContext.Current.Services.TextService.Localize($"qrCodeLevels/{l}Name"), description = ApplicationContext.Current.Services.TextService.Localize($"qrCodeLevels/{l}Description") });

            return Ok(result);
        }


    }
}