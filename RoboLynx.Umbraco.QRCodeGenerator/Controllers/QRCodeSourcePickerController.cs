using System.Web.Http;
using Umbraco.Web.Editors;
using System.Linq;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources;
using Umbraco.Core.Persistence;
using Umbraco.Web;
using Umbraco.Core.Configuration;
using Umbraco.Core.Logging;
using Umbraco.Core.Cache;
using Umbraco.Core.Services;
using Umbraco.Core;

namespace RoboLynx.Umbraco.QRCodeGenerator.Controllers
{
    [PluginController(Constants.PluginAlias)]
    [AngularJsonOnlyConfiguration]
    public class QRCodeSourcePickerController : UmbracoAuthorizedJsonController
    {
        private readonly QRCodeSourcesCollection sources;

        public QRCodeSourcePickerController(QRCodeSourcesCollection sources, IGlobalSettings globalSettings, IUmbracoContextAccessor umbracoContextAccessor,
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
            this.sources = sources;
        }

        [HttpGet]
        public IHttpActionResult Get()
        {
            var result = sources.Select(ct => new { id = ct.Id, name = ct.Name, description = ct.Description });
            
            return Ok(result);
        }
    }
}