using Chronos.Abstractions;
using System;
using System.IO;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Cache;
using Umbraco.Core.Logging;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.Sync;
using Umbraco.Web;
using Umbraco.Web.Scheduling;

namespace RoboLynx.Umbraco.QRCodeGenerator.Cache
{
    public class QRCodeCache<T> : IQRCodeCache, IDisposable
    {
        private readonly IProfilingLogger _logger;
        private readonly IDateTimeOffsetProvider _dateTimeProvider;
        private readonly IRuntimeState _runtime;
        private readonly BackgroundTaskRunner<IBackgroundTask> _cleanCacheRunner;
        private readonly IAppPolicyCache _isolatedCache;
        private readonly IQRCodeCacheFileSystem _fileSystem;
        private readonly IQRCodeCacheUrlProvider _urlProvider;

        public QRCodeCache(string name, AppCaches appCaches, IQRCodeCacheFileSystem fileSystem, IQRCodeCacheUrlProvider urlProvider,
            IRuntimeState runtime, IProfilingLogger logger, IDateTimeOffsetProvider dateTimeProvider)
        {
            _isolatedCache = appCaches.IsolatedCaches.GetOrCreate<T>();
            _cleanCacheRunner = new BackgroundTaskRunner<IBackgroundTask>("CleanQRCodeCache-" + name, logger);
            Name = name;
            _logger = logger;
            _dateTimeProvider = dateTimeProvider;
            _runtime = runtime;
            _fileSystem = fileSystem;
            _urlProvider = urlProvider;

            Initialize();
        }

        public int DelayBeforeWeStart { get; set; } = 60000; // 60000ms = 1min
        public int HowOftenWeRepeat { get; set; } = 3600000; // 3600000 = 1hour
        public string Name { get; }
        public TimeSpan Timeout { get; }

        /// <inheritdoc/>
        public void Add(string hashId, string extension, Stream stream)
        {
            if (string.IsNullOrEmpty(extension))
            {
                throw new ArgumentException($"'{nameof(extension)}' cannot be null or empty.", nameof(extension));
            }

            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            stream.Seek(0, SeekOrigin.Begin);

            var cachedFile = _fileSystem.AddCacheFile(hashId, extension, stream);

            var timeout = cachedFile.ExpiryDate.UtcDateTime - _dateTimeProvider.UtcNow;
            _isolatedCache.InsertCacheItem(hashId, () => new FileCacheData
            {
                HashId = hashId,
                Path = cachedFile.Path,
                ExpiryDate = cachedFile.ExpiryDate
            },
            timeout);
        }

        /// <inheritdoc/>
        public void Clear(string hashId)
        {
            var cacheItem = GetCacheItem(hashId);

            _fileSystem.DeleteCacheFiles(cacheItem.Path.AsEnumerableOfOne());

            _isolatedCache.Clear(hashId);
        }

        /// <inheritdoc/>
        public void ClearAll()
        {
            var files = _fileSystem.GetAllCacheFiles();
            _fileSystem.DeleteCacheFiles(files.Select(f => f.Path));

            _isolatedCache.Clear();
        }

        /// <inheritdoc/>
        public Stream GetStream(string hashId)
        {
            var cacheItem = GetCacheItem(hashId);

            if (_fileSystem.FileExists(cacheItem.Path))
            {
                return _fileSystem.OpenFile(cacheItem.Path);
            }
            else
            {
                Clear(hashId);
            }

            return null;
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

            return cacheItem != null && cacheItem.ExpiryDate.UtcDateTime > _dateTimeProvider.UtcNow;
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
            if (string.IsNullOrWhiteSpace(hashId))
            {
                throw new ArgumentException($"'{nameof(hashId)}' cannot be null or whitespace.", nameof(hashId));
            }

            return _isolatedCache.GetCacheItem<FileCacheData>(hashId);
        }

        /// <summary>
        /// Initialize cache.
        /// </summary>
        protected void Initialize()
        {
            UpdateRuntimeCache();
            InitializeTaskRunner();
        }

        /// <summary>
        /// Initialize task runner to periodically clean the cache.
        /// </summary>
        private void InitializeTaskRunner()
        {
            var task = new CleanCacheTask<T>(_cleanCacheRunner, DelayBeforeWeStart, HowOftenWeRepeat, _runtime, _logger, this);

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

        public void Dispose()
        {
            _cleanCacheRunner.Dispose();
        }
    }
}
