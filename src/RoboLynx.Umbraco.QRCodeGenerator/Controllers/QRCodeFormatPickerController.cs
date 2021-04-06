using RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat;
using System.Linq;
using System.Web.Http;
using Umbraco.Core;
using Umbraco.Core.Cache;
using Umbraco.Core.Configuration;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;

namespace RoboLynx.Umbraco.QRCodeGenerator.Controllers
{
    [PluginController(Constants.PluginAlias)]
    [AngularJsonOnlyConfiguration]
    public class QRCodeFormatPickerController : UmbracoAuthorizedJsonController
    {
        private readonly QRCodeFormatsCollection formats;

        public QRCodeFormatPickerController(QRCodeFormatsCollection formats, IGlobalSettings globalSettings, IUmbracoContextAccessor umbracoContextAccessor,
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
            this.formats = formats;
        }

        [HttpGet]
        public IHttpActionResult Get()
        {
            var result = formats.Select(ct => new { id = ct.Id, name = ct.Name });

            return Ok(result);
        }


    }
}