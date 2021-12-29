using Chronos.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;
using Umbraco.Core.Cache;
using Umbraco.Core.Logging;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace RoboLynx.Umbraco.QRCodeGenerator.Cache
{
    public class QRCodeCacheFactory : IQRCodeCacheFactory
    {
        private readonly AppCaches _appCaches;
        private readonly IProfilingLogger _logger;
        private readonly IRuntimeState _runtimeState;
        private readonly IDateTimeOffsetProvider _dateTimeProvider;

        public QRCodeCacheFactory(AppCaches appCaches, IProfilingLogger logger, IRuntimeState runtimeState, IDateTimeOffsetProvider dateTimeProvider)
        {
            _appCaches = appCaches;
            _logger = logger;
            _runtimeState = runtimeState;
            _dateTimeProvider = dateTimeProvider;
        }

        public IQRCodeCache CreateCache<T>(string name, IQRCodeCacheFileSystem fileSystem, IQRCodeCacheUrlProvider urlProvider)
        {
            return new QRCodeCache<T>(name,
                    _appCaches,
                    fileSystem,
                    urlProvider,
                    _runtimeState,
                    _logger, 
                    _dateTimeProvider);
        }
    }
}
