using RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Umbraco.Core;
using Umbraco.Core.Cache;
using Umbraco.Core.Configuration;
using Umbraco.Core.Logging;
using Umbraco.Core.Models.Identity;
using Umbraco.Core.Persistence;
using Umbraco.Core.Security;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;

namespace RoboLynx.Umbraco.QRCodeGenerator.Controllers
{
    [PluginController(Constants.PluginAlias)]
    [AngularJsonOnlyConfiguration]
    public class QRCodeSourcePickerController : UmbracoAuthorizedJsonController
    {
        private readonly QRCodeSourcesCollection sources;

        public QRCodeSourcePickerController(QRCodeSourcesCollection sources, UmbracoContext umbracoContext, UmbracoHelper umbracoHelper,
                                 BackOfficeUserManager<BackOfficeIdentityUser> backOfficeUserManager) : base(umbracoContext, umbracoHelper, backOfficeUserManager)
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