using RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat;
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
    public class QRCodeFormatPickerController : UmbracoAuthorizedJsonController
    {
        private readonly QRCodeFormatsCollection formats;

        public QRCodeFormatPickerController(QRCodeFormatsCollection formats, 
                                            UmbracoContext umbracoContext, UmbracoHelper umbracoHelper,
                                            BackOfficeUserManager<BackOfficeIdentityUser> backOfficeUserManager) : base(umbracoContext, umbracoHelper, backOfficeUserManager)
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