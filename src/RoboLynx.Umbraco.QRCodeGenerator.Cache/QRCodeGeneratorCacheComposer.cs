using Chronos.Abstractions;
using RoboLynx.Umbraco.QRCodeGenerator.Helpers;
using System;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.Exceptions;
using Umbraco.Core.IO;
using Umbraco.Core.Logging;

namespace RoboLynx.Umbraco.QRCodeGenerator.Cache
{
    public class QRCodeGeneratorCacheComposer : IUserComposer
    {
        public string CacheName => Constants.BackofficeCacheName;


        public void Compose(Composition composition)
        {
            var config = CreateConfiguration();

            if (!config.Disable)
            {
                composition.Register<IQRCodeCacheFileSystem>(f => new QRCodeCacheFileSystem(new PhysicalFileSystem(config.CacheLocation), TimeSpan.FromDays(config.MaxDays), f.GetInstance<ILogger>(), f.GetInstance<IDateTimeOffsetProvider>()));
                composition.RegisterAuto<IQRCodeCache>();
            }
        }

        private QRCodeCacheConfig CreateConfiguration()
        {
            var cacheLoaction = ConfigurationHelper.GetAppSetting(Constants.Configuration.LocationKey, CacheName) ?? Constants.DefaultLocalCacheLocation;

            var disable = ConfigurationHelper.GetAppSetting(Constants.Configuration.DisableKey, CacheName) != null
                            && ConfigurationHelper.GetAppSetting(Constants.Configuration.DisableKey, CacheName)
                            .Equals("true", StringComparison.InvariantCultureIgnoreCase);

            var maxDays = double.Parse(ConfigurationHelper.GetAppSetting(Constants.Configuration.MaxDaysKey, CacheName) ?? "0");

            //Check we have all values set - otherwise make sure Umbraco does NOT boot so it can be configured correctly
            if (string.IsNullOrEmpty(cacheLoaction))
                throw new ArgumentNullOrEmptyException(Constants.Configuration.LocationKey, $"The QR Code Generator is missing the value '{Constants.Configuration.LocationKey}:{CacheName}' from AppSettings");

            return new QRCodeCacheConfig
            {
                Disable = disable,
                CacheLocation = cacheLoaction,
                MaxDays = maxDays
            };
        }
    }
}
