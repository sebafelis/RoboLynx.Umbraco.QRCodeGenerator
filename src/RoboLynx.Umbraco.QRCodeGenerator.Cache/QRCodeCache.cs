﻿using Chronos.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Sync;
using Umbraco.Extensions;

namespace RoboLynx.Umbraco.QRCodeGenerator.Cache
{
    public class QRCodeCache<T> : IQRCodeCache
    {
        private readonly IProfilingLogger _profilingLogger;
        private readonly IDateTimeOffsetProvider _dateTimeProvider;
        private readonly IServerRoleAccessor _serverRoleAccessor;
        private readonly ILogger<QRCodeCache<T>> _logger;
        private readonly IAppPolicyCache _isolatedCache;
        private readonly IQRCodeCacheFileSystem _fileSystem;
        private readonly IQRCodeCacheUrlProvider _urlProvider;

        public QRCodeCache(AppCaches appCaches, IQRCodeCacheFileSystem fileSystem, IQRCodeCacheUrlProvider urlProvider,
            IProfilingLogger profilingLogger, ILogger<QRCodeCache<T>> logger, IDateTimeOffsetProvider dateTimeProvider,
            IServerRoleAccessor serverRoleAccessor)
        {
            var cacheInsance = (IQRCodeCacheRole)Activator.CreateInstance<T>();
            Name = cacheInsance.Name;

            _isolatedCache = appCaches.IsolatedCaches.GetOrCreate<T>();           
            _profilingLogger = profilingLogger;
            _dateTimeProvider = dateTimeProvider;
            _serverRoleAccessor = serverRoleAccessor;
            _logger = logger;
            _fileSystem = fileSystem;
            _urlProvider = urlProvider;

            Initialize();
        }

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
                ExpiryDate = cachedFile.ExpiryDate,
                LastModifiedDate = cachedFile.LastModifiedDate
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
            using var profiler = _profilingLogger.TraceDuration<QRCodeCache<T>>("Cache cleaning started", "Cache cleaned");

            var expiredItems = _isolatedCache.GetCacheItemsByKeySearch<FileCacheData>("").Where(item => item.ExpiryDate < _dateTimeProvider.UtcNow);

            if (expiredItems.Any())
            {
                using (_profilingLogger.TraceDuration<QRCodeCache<T>>("Deleting expired cache files.", "Expired cache files are deleted."))
                {
                    _fileSystem.DeleteCacheFiles(expiredItems.Select(s => s.Path));
                }

                foreach (var item in expiredItems)
                {
                    _isolatedCache.Clear(item.HashId);
                }
            }
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
        }

        /// <summary>
        /// Update runtime cache by existing file cache.
        /// </summary>
        private void UpdateRuntimeCache()
        {
            switch (_serverRoleAccessor.CurrentServerRole)
            {
                case ServerRole.Subscriber:
                    _logger.LogDebug("UpdateRuntimeCache does not run on replica servers.");
                    return;
                case ServerRole.Unknown:
                    _logger.LogDebug("UpdateRuntimeCache does not run on servers with unknown role.");
                    return;
                default:
                    using (_profilingLogger.TraceDuration<QRCodeCache<T>>("Updating runtime cache.", "Cache was update."))
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

        public DateTimeOffset? LastModified(string hashId)
        {
            return GetCacheItem(hashId)?.LastModifiedDate;
        }
    }
}
