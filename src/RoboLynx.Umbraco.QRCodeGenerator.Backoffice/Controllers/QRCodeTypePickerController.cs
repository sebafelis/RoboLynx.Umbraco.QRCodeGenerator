using Microsoft.AspNetCore.Mvc;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes;
using System.Linq;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco.Cms.Web.Common.Attributes;
using Umbraco.Cms.Web.Common.Filters;

namespace RoboLynx.Umbraco.QRCodeGenerator.Controllers
{
    [PluginController(Core.PluginAlias)]
    [AngularJsonOnlyConfiguration]
    public class QRCodeTypePickerController : UmbracoAuthorizedJsonController
    {
        private readonly QRCodeTypeFactoryCollection _types;

        public QRCodeTypePickerController(QRCodeTypeFactoryCollection types)
        {
            this._types = types;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var result = _types.Select(ct => new { id = ct.Id, name = ct.Name, description = ct.Description });

            return Ok(result);
        }
    }
}