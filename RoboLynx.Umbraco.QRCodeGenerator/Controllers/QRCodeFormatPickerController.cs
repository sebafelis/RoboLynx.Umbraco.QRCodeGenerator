using System.Web.Http;
using Umbraco.Web.Editors;
using System.Linq;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;
using RoboLynx.Umbraco.QRCodeGenerator.Helpers;

namespace RoboLynx.Umbraco.QRCodeGenerator.Controllers
{
    [PluginController(RoboLynx.Umbraco.QRCodeGenerator.Constants.PluginName)]
    [AngularJsonOnlyConfiguration]
    public class QRCodeFormatPickerController : UmbracoAuthorizedJsonController
    {
        [HttpGet]
        public IHttpActionResult Get()
        {
            var result = QRCodeHelper.GetQRCodeFormatProviders().Select(ct => new { id = ct.Id, name = ct.Name });
            
            return Ok(result);
        }

     
    }
}