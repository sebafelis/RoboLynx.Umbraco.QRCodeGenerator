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
    public class QRCodeTypePickerController : UmbracoAuthorizedJsonController
    {
        [HttpGet]
        public IHttpActionResult Get()
        {
            var result = QRCodeHelper.GetQRCodeTypeProviders().Select(ct => new { id = ct.GetType().Name, name = ct.Name, description = ct.Description });
            
            return Ok(result);
        }

     
    }
}