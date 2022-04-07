using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Sync;
using Umbraco.Cms.Infrastructure.HostedServices;

namespace RoboLynx.Umbraco.QRCodeGenerator.Cache
{
    public class RecurringCleanupCache : RecurringHostedServiceBase
    {
        private readonly IQRCodeCacheManager _cacheManager;
        private readonly string _cacheName;
        private readonly IServerRoleAccessor _serverRoleAccessor;
        private readonly ILogger<RecurringCleanupCache> _logger;

        public RecurringCleanupCache(string cacheName, IOptionsMonitor<QRCodeCacheOptions> cacheOptions,
            IServerRoleAccessor serverRoleAccessor, IQRCodeCacheManager cacheManager, ILogger<RecurringCleanupCache> logger)
                : base(cacheOptions.Get(cacheName).PeriodCleanCache, cacheOptions.Get(cacheName).DelayCleanCache)
        {
            _cacheName = cacheName;
            _serverRoleAccessor = serverRoleAccessor;
            _cacheManager = cacheManager;
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

            _cacheManager.CleanupCache(_cacheName);

            return Task.CompletedTask;
        }
    }
}