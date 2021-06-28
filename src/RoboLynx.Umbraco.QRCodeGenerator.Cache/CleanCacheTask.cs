using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;
using Umbraco.Core.Cache;
using Umbraco.Core.Logging;
using Umbraco.Core.Sync;
using Umbraco.Web.Scheduling;

namespace RoboLynx.Umbraco.QRCodeGenerator.Cache
{
    public class CleanCacheTask<T> : RecurringTaskBase
    {
        private readonly IRuntimeState _runtime;
        private readonly IProfilingLogger _logger;
        private readonly IQRCodeCache _cache;

        public CleanCacheTask(IBackgroundTaskRunner<RecurringTaskBase> runner, int delayBeforeWeStart, int howOftenWeRepeat, 
            IRuntimeState runtime, IProfilingLogger logger, IQRCodeCache cache)
            : base(runner, delayBeforeWeStart, howOftenWeRepeat)
        {
            _runtime = runtime;
            _logger = logger;
            _cache = cache;
        }

        public override bool PerformRun()
        {
            // Do not run the code on replicas nor unknown role servers
            // ONLY run for Master server or Single
            switch (_runtime.ServerRole)
            {
                case ServerRole.Replica:
                    _logger.Debug<CleanCacheTask<T>>("Does not run on replica servers.");
                    return true; // We return true to try again as the server role may change!
                case ServerRole.Unknown:
                    _logger.Debug<CleanCacheTask<T>>("Does not run on servers with unknown role.");
                    return true; // We return true to try again as the server role may change!
            }

            _cache.CleanupCache();

            // If we want to keep repeating - we need to return true
            // But if we run into a problem/error & want to stop repeating - return false
            return true;
        }

        public override bool IsAsync => false;
    }
}
