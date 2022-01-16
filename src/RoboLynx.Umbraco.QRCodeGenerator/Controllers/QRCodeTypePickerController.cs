using Microsoft.AspNetCore.Mvc;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes;
using System.Linq;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco.Cms.Web.Common;
using Umbraco.Cms.Web.Common.Attributes;
using Umbraco.Cms.Web.Common.Filters;

namespace RoboLynx.Umbraco.QRCodeGenerator.Controllers
{
    [PluginController(Constants.Core.PluginAlias)]
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