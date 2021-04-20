using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes;
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
    public class QRCodeTypePickerController : UmbracoAuthorizedJsonController
    {
        private readonly QRCodeTypesCollection types;

        public QRCodeTypePickerController(QRCodeTypesCollection types, 
                                UmbracoContext umbracoContext, UmbracoHelper umbracoHelper,
                                 BackOfficeUserManager<BackOfficeIdentityUser> backOfficeUserManager) : base(umbracoContext, umbracoHelper, backOfficeUserManager)
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