using Chronos.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;
using Umbraco.Core.IO;
using Umbraco.Core.Logging;

namespace RoboLynx.Umbraco.QRCodeGenerator.Cache
{
    public class QRCodeCacheFileSystemFactory : IQRCodeCacheFileSystemFactory
    {
        private readonly ILogger _logger;
        private readonly IDateTimeOffsetProvider _dateTimeOffsetProvider;

        public QRCodeCacheFileSystemFactory(ILogger logger, IDateTimeOffsetProvider dateTimeOffsetProvider)
        {
            _logger = logger;
            _dateTimeOffsetProvider = dateTimeOffsetProvider;
        }

        public IQRCodeCacheFileSystem CreateFileSystem(IFileSystem innerFileSystem, TimeSpan expirationTimeSpan)
        {
            return new QRCodeCacheFileSystem(innerFileSystem, expirationTimeSpan, _logger, _dateTimeOffsetProvider);
        }
    }
}
