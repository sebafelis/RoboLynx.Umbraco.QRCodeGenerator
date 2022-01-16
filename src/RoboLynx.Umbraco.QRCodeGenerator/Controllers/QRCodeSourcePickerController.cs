using Microsoft.AspNetCore.Mvc;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources;
using System.Linq;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco.Cms.Web.Common.Attributes;
using Umbraco.Cms.Web.Common.Filters;

namespace RoboLynx.Umbraco.QRCodeGenerator.Controllers
{
    [PluginController(Constants.Core.PluginAlias)]
    [AngularJsonOnlyConfiguration]
    public class QRCodeSourcePickerController : UmbracoAuthorizedJsonController
    {
        private readonly QRCodeSourceFactoryCollection _sources;

        public QRCodeSourcePickerController(QRCodeSourceFactoryCollection sources)
        {
            this._sources = sources;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var result = _sources.Select(ct => new { id = ct.Id, name = ct.Name, description = ct.Description });

            return Ok(result);
        }
    }
}