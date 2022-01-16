using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Scoping;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Sync;
using Umbraco.Cms.Infrastructure.HostedServices;

namespace RoboLynx.Umbraco.QRCodeGenerator.Cache
{
    public class CleanCacheHostedService : RecurringHostedServiceBase
    {
        private readonly IQRCodeCache _cache;
        private readonly string _cacheName;
        private readonly IServerRoleAccessor _serverRoleAccessor;
        private readonly ILogger<CleanCacheHostedService> _logger;

        public CleanCacheHostedService(string cacheName, IOptionsSnapshot<QRCodeCacheOptions> cacheOptions,
            IServerRoleAccessor serverRoleAccessor, IQRCodeCache cache, ILogger<CleanCacheHostedService> logger)
                : base(cacheOptions.Get(cacheName).PeriodCleanCache, cacheOptions.Get(cacheName).DelayCleanCache)
        {
            _cacheName = cacheName;
            _serverRoleAccessor = serverRoleAccessor;
            _cache = cache;
            _logger = logger;
        }


        public override Task PerformExecuteAsync(object state)
        {
            _logger.LogInformation($"Cleaning {_cacheName} QR code cache.");

            // Do not run the code on subscribers or unknown role servers
            // ONLY run for SchedulingPublisher server or Single server roles
            switch (_serverRoleAccessor.CurrentServerRole)
            {
                case ServerRole.Subscriber:
                    _logger.LogDebug("Does not run on subscriber servers.");
                    return Task.CompletedTask; // We return Task.CompletedTask to try again as the server role may change!
                case ServerRole.Unknown:
                    _logger.LogDebug("Does not run on servers with unknown role.");
                    return Task.CompletedTask; // We return Task.CompletedTask to try again as the server role may change! 
            }

           _cache.CleanupCache();

           return Task.CompletedTask;
        }       
    }
}
