using Microsoft.AspNetCore.Mvc;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat;
using System.Linq;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco.Cms.Web.Common.Attributes;
using Umbraco.Cms.Web.Common.Filters;

namespace RoboLynx.Umbraco.QRCodeGenerator.Controllers
{
    [PluginController(Constants.Core.PluginAlias)]
    [AngularJsonOnlyConfiguration]
    public class QRCodeFormatPickerController : UmbracoAuthorizedJsonController
    {
        private readonly QRCodeFormatFactoryCollection _formats;

        public QRCodeFormatPickerController(QRCodeFormatFactoryCollection formats)
        {
            _formats = formats;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var result = _formats.Select(ct => new { id = ct.Id, name = ct.Name });

            return Ok(result);
        }
    }
}