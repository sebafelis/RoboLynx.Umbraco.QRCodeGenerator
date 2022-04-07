using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Sync;
using Umbraco.Cms.Infrastructure.HostedServices;

namespace RoboLynx.Umbraco.QRCodeGenerator.Cache
{
    public class CacheInitializeTask : RecurringHostedServiceBase
    {
        private static readonly TimeSpan _period = TimeSpan.FromSeconds(30);
        private static readonly TimeSpan _delay = TimeSpan.FromMinutes(1);

        private readonly string _cacheName;
        private readonly IServerRoleAccessor _serverRoleAccessor;
        private readonly IQRCodeCacheManager _qrCodeCacheManager;
        private readonly ILogger<CacheInitializeTask> _logger;

        public CacheInitializeTask(string cacheName, IServerRoleAccessor serverRoleAccessor, IQRCodeCacheManager qrCodeCacheManager,
            ILogger<CacheInitializeTask> logger) : base(_period, _delay)
        {
            _cacheName = cacheName;
            _serverRoleAccessor = serverRoleAccessor;
            _qrCodeCacheManager = qrCodeCacheManager;
            _logger = logger;
        }

        public override Task PerformExecuteAsync(object state)
        {
            _logger.LogInformation($"Checking {_cacheName} QR code cache for existing items.");

            // Do not run the code on subscribers or unknown role servers
            // ONLY run for SchedulingPublisher server or Single server roles
            switch (_serverRoleAccessor.CurrentServerRole)
            {
                case ServerRole.Subscriber:
                    StopAsync(System.Threading.CancellationToken.None);
                    _logger.LogDebug("Does not run on servers with subscriber role.");
                    return Task.CompletedTask;

                case ServerRole.Unknown:
                    _logger.LogDebug("Does not run on servers with unknown role. Waiting for known state.");
                    return Task.CompletedTask;
            }

            _qrCodeCacheManager.Initialize(_cacheName);

            StopAsync(System.Threading.CancellationToken.None);

            _logger.LogInformation($"{_cacheName} QR code cache initialized.");

            return Task.CompletedTask;
        }
    }
}