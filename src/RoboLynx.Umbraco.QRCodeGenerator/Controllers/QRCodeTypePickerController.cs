using System.Web.Http;
using Umbraco.Web.Editors;
using System.Linq;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes;
using Umbraco.Core.Configuration;
using Umbraco.Web;
using Umbraco.Core.Persistence;
using Umbraco.Core.Logging;
using Umbraco.Core.Cache;
using Umbraco.Core.Services;
using Umbraco.Core;

namespace RoboLynx.Umbraco.QRCodeGenerator.Controllers
{
    [PluginController(Constants.PluginAlias)]
    [AngularJsonOnlyConfiguration]
    public class QRCodeTypePickerController : UmbracoAuthorizedJsonController
    {
        private readonly QRCodeTypesCollection types;

        public QRCodeTypePickerController(QRCodeTypesCollection types, IGlobalSettings globalSettings, IUmbracoContextAccessor umbracoContextAccessor,
                                ISqlContext sqlContext, ServiceContext services, AppCaches appCaches,
                                IProfilingLogger logger, IRuntimeState runtimeState, UmbracoHelper umbracoHelper) : base(globalSettings,
                                                                                                                         umbracoContextAccessor,
                                                                                                                         sqlContext,
                                                                                                                         services,
                                                                                                                         appCaches,
                                                                                                                         logger,
                                                                                                                         runtimeState,
                                                                                                                         umbracoHelper)
        { 
            this.types = types;
        }

        [HttpGet]
        public IHttpActionResult Get()
        {
            var result = types.Select(ct => new { id = ct.Id, name = ct.Name, description = ct.Description });
            
            return Ok(result);
        }

     
    }
}