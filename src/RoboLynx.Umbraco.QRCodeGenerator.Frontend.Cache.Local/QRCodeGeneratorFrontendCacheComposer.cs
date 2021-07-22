using Chronos.Abstractions;
using RoboLynx.Umbraco.QRCodeGenerator.Cache;
using RoboLynx.Umbraco.QRCodeGenerator.Helpers;
using System;
using Umbraco.Core;
using Umbraco.Core.Cache;
using Umbraco.Core.Composing;
using Umbraco.Core.Exceptions;
using Umbraco.Core.IO;
using Umbraco.Core.Logging;
using CacheConstants = RoboLynx.Umbraco.QRCodeGenerator.Cache.Constants;
using FrontendConstants = RoboLynx.Umbraco.QRCodeGenerator.Frontend.Constants;

namespace RoboLynx.Umbraco.QRCodeGenerator.Frontend.Cache
{
    [ComposeBefore(typeof(QRCodeGeneratorCoreComposer))]
    public class QRCodeGeneratorFrontendCacheComposer : IUserComposer
    {
        private string CacheName => FrontendConstants.FrontendCacheName;


        public void Compose(Composition composition)
        {
            var config = CreateConfiguration();

            if (!config.Disable)
            {
                composition.RegisterFor<IQRCodeCache, IFrontendQRCodeCache>(f=> new QRCodeCache<IFrontendQRCodeCache>(CacheName,
                    f.GetInstance<AppCaches>(), new QRCodeCacheFileSystem(new PhysicalFileSystem(config.CacheLocation), 
                    TimeSpan.FromDays(config.MaxDays), f.GetInstance<ILogger>(), f.GetInstance<IDateTimeOffsetProvider>()), 
                    null, f.GetInstance<IRuntimeState>(), 
                    f.GetInstance<IProfilingLogger>(), f.GetInstance<IDateTimeOffsetProvider>()));
            }
        }

        private QRCodeCacheConfig CreateConfiguration()
        {
            var cacheLoaction = ConfigurationHelper.GetAppSetting(CacheConstants.Configuration.LocationKey, CacheName) ?? Constants.DefaultFrontendCacheLocation;

            var disable = ConfigurationHelper.GetAppSetting(CacheConstants.Configuration.DisableKey, CacheName) != null
                            && ConfigurationHelper.GetAppSetting(CacheConstants.Configuration.DisableKey, CacheName)
                            .Equals("true", StringComparison.InvariantCultureIgnoreCase);

            var maxDays = double.Parse(ConfigurationHelper.GetAppSetting(CacheConstants.Configuration.MaxDaysKey, CacheName) ?? "0");

            //Check we have all values set - otherwise make sure Umbraco does NOT boot so it can be configured correctly
            if (string.IsNullOrEmpty(cacheLoaction))
                throw new ArgumentNullOrEmptyException(CacheConstants.Configuration.LocationKey, $"The QR Code Generator is missing the value '{CacheConstants.Configuration.LocationKey}:{CacheName}' from AppSettings");

            return new QRCodeCacheConfig
            {
                Disable = disable,
                CacheLocation = cacheLoaction,
                MaxDays = maxDays
            };
        }
    }
}
