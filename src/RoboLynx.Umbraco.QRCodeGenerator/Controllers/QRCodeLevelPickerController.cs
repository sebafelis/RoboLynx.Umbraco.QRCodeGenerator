﻿using System;
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
    public class QRCodeLevelPickerController : UmbracoAuthorizedJsonController
    {
        private readonly ILocalizedTextService localizedTextService;

        public QRCodeLevelPickerController(ILocalizedTextService localizedTextService, IGlobalSettings globalSettings, IUmbracoContextAccessor umbracoContextAccessor,
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
            this.localizedTextService = localizedTextService;
        }

        [HttpGet]
        public IHttpActionResult Get()
        {
            var eccLevelValues = Enum.GetValues(typeof(ECCLevel)).Cast<ECCLevel>();
            var result = eccLevelValues.Select(l => new { id = l.ToString(), name = localizedTextService.Localize($"qrCodeLevels/{l}Name"), description = localizedTextService.Localize($"qrCodeLevels/{l}Description") });

            return Ok(result);
        }


    }
}