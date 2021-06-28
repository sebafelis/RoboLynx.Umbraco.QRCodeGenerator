using Chronos.Abstractions;
using System;
using System.IO;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Cache;
using Umbraco.Core.Logging;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.Sync;
using Umbraco.Web.Scheduling;

namespace RoboLynx.Umbraco.QRCodeGenerator.Cache
{
    public class QRCodeCache<T> : IQRCodeCache
    {
        private readonly IProfilingLogger _logger;
        private readonly IDateTimeOffsetProvider _dateTimeProvider;
        private readonly IRuntimeState _runtime;
        private readonly BackgroundTaskRunner<IBackgroundTask> _cleanCacheRunner;
        private readonly IAppPolicyCache _isolatedCache;
        private readonly IQRCodeCacheFileSystem _fileSystem;
        private readonly QRCodeCacheRefresher<T> _cacheRefresher;
        private readonly IQRCodeCacheUrlProvider _urlProvider;

        public QRCodeCache(IProfilingLogger logger, IDateTimeOffsetProvider dateTimeProvider, AppCaches appCaches, IRuntimeState runtime,
            IQRCodeCacheFileSystem fileSystem, QRCodeCacheRefresher<T> cacheRefresher, IQRCodeCacheUrlProvider urlProvider)
        {
            _isolatedCache = appCaches.IsolatedCaches.GetOrCreate<T>();
            _cleanCacheRunner = new BackgroundTaskRunner<IBackgroundTask>("CleanQRCodeCache", logger);
            this._logger = logger;
            this._dateTimeProvider = dateTimeProvider;
            this._runtime = runtime;
            this._fileSystem = fileSystem;
            this._cacheRefresher = cacheRefresher;
            this._urlProvider = urlProvider;
        }

        public int DelayBeforeWeStart { get; set; } = 60000; // 60000ms = 1min
        public int HowOftenWeRepeat { get; set; } = 3600000; // 3600000 = 1hour

        /// <inheritdoc/>
        public void Initialize()
        {
            UpdateRuntimeCache();
            InitializeTaskRunner();
        }

        /// <inheritdoc/>
        public void Add(string hashId, string extension, Stream stream)
        {
            if (!IsCached(hashId))
            {
                //TODO Change format source to property value from QRCodeFormat class getting by Id.
                var expiryDateFiles = _fileSystem.AddCacheFile(hashId, extension, stream);

                var runtimeCacheRefreshData = new QRCodeCacheRefresher<T>.JsonPayload(hashId, QRCodeCacheChangeType.Add, expiryDateFiles.Path, expiryDateFiles.ExpiryDate);

                _cacheRefresher.Refresh(runtimeCacheRefreshData.AsEnumerableOfOne().ToArray());
            }
        }

        /// <inheritdoc/>
        public void Clear(string hashId)
        {
            var runtimeCacheRefreshData = new QRCodeCacheRefresher<T>.JsonPayload(hashId, QRCodeCacheChangeType.Remove);

            _cacheRefresher.Refresh(runtimeCacheRefreshData.AsEnumerableOfOne().ToArray());
        }

        /// <inheritdoc/>
        public void ClearAll()
        {
            _cacheRefresher.RefreshAll();
        }

        /// <inheritdoc/>
        public Stream GetStream(string hashId)
        {
            var cacheItem = GetCacheItem(hashId);

            if (_fileSystem.FileExists(cacheItem.Path))
            {
                throw new FileNotFoundException();
            }

            return _fileSystem.OpenFile(cacheItem.Path);
        }

        /// <inheritdoc/>
        public string Url(string hashId, UrlMode urlMode)
        {
            var cacheItem = GetCacheItem(hashId);

            return _urlProvider.Url(cacheItem.Path, urlMode);
        }

        /// <inheritdoc/>
        public bool IsCached(string hashId)
        {
            var cacheItem = GetCacheItem(hashId);

            return cacheItem != null && cacheItem.ExpiryDate > _dateTimeProvider.UtcNow;
        }

        /// <inheritdoc/>
        public void CleanupCache()
        {
            //var expiredFiles = fileSystem.GetExpiredCacheFiles();
            //var expiredFilesCount = expiredFiles.Count();

            //logger.Info<CleanCacheTask<T>>("QR Code file cache cleaner - {ServerRole}", runtime.ServerRole);
            //logger.Info<CleanCacheTask<T>>("{expiredFilesCount} files is expired.", expiredFilesCount);
            _logger.Info<CleanCacheTask<T>>("QRCode cache cleanup is running. Server role: {ServerRole}", _runtime.ServerRole);

            var expiredItems = _isolatedCache.GetCacheItemsByKeySearch<FileCacheData>("").Where(item => item.ExpiryDate < _dateTimeProvider.UtcNow);

            if (expiredItems.Any())
            {
                using (_logger.TraceDuration<CleanCacheTask<T>>("Deleting expired cache files.", "Expired cache files are deleted."))
                {
                    _fileSystem.DeleteCacheFiles(expiredItems.Select(s => s.Path));
                }

                foreach (var item in expiredItems)
                {
                    _isolatedCache.Clear(item.HashId);
                }
            }

            _logger.Info<CleanCacheTask<T>>("QRCode cache cleanup finished.");
        }

        private FileCacheData GetCacheItem(string hashId)
        {
            return _isolatedCache.GetCacheItem<FileCacheData>(hashId);
        }

        /// <summary>
        /// Initialize task runner to periodically clean the cache.
        /// </summary>
        private void InitializeTaskRunner()
        {
            var task = new CleanCacheTask<T>(_cleanCacheRunner, DelayBeforeWeStart, HowOftenWeRepeat, _runtime, _logger, this);

            //As soon as we add our task to the runner it will start to run (after its delay period)
            _cleanCacheRunner.TryAdd(task);
        }

        /// <summary>
        /// Update runtime cache by existing file cache.
        /// </summary>
        private void UpdateRuntimeCache()
        {
            switch (_runtime.ServerRole)
            {
                case ServerRole.Replica:
                    _logger.Debug<QRCodeCache<T>>("UpdateRuntimeCache does not run on replica servers.");
                    return;
                case ServerRole.Unknown:
                    _logger.Debug<QRCodeCache<T>>("UpdateRuntimeCache does not run on servers with unknown role.");
                    return;
                default:
                    using (_logger.TraceDuration<CleanCacheTask<T>>("Updating runtime cache.", "Cache was update."))
                    {
                        foreach (var file in _fileSystem.GetAllCacheFiles())
                        {
                            _isolatedCache.GetCacheItem(file.HashId, () => file);
                        }
                    }
                    break;
            }
        }

        /// <inheritdoc/>
        public bool UrlSupport()
        {
            return _urlProvider != null;
        }

        /// <inheritdoc/>
        public DateTimeOffset? Expired(string hashId)
        {
            return GetCacheItem(hashId)?.ExpiryDate;
        }
    }
}
