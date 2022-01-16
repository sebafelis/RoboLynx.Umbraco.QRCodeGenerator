using Chronos.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Hosting;
using Umbraco.Cms.Core.IO;

namespace RoboLynx.Umbraco.QRCodeGenerator.Cache
{
    public class QRCodeCacheFileSystem : IQRCodeCacheFileSystem
    {
        const string _defaultPath = "";
        private readonly IFileSystem _innerFileSystem;
        private readonly IOptionsMonitor<QRCodeCacheOptions> _options;
        private readonly ILogger _logger;
        private readonly IDateTimeOffsetProvider _dateTimeProvider;

        public QRCodeCacheFileSystem(IFileSystem innerFileSystem, IOptionsMonitor<QRCodeCacheOptions> options,
            IDateTimeOffsetProvider dateTimeProvider, ILogger<QRCodeCacheFileSystem> logger)
        {
            _innerFileSystem = innerFileSystem;
            _options = options;
            _logger = logger;
            _dateTimeProvider = dateTimeProvider;
        }

        public TimeSpan ExpirationTimeSpan => TimeSpan.FromDays(_options.CurrentValue.MaxDays);

        public bool CanAddPhysical => _innerFileSystem.CanAddPhysical;

        public IEnumerable<FileCacheData> GetExpiredCacheFiles(string path = null)
        {
            if (string.IsNullOrEmpty(path))
                path = _defaultPath;

            var files = GetFiles(path);

            foreach (var file in files)
            {
                var expiryDate = GetExpiryDate(file);
                if (expiryDate < _dateTimeProvider.UtcNow)
                {
                    yield return new FileCacheData
                    {
                        HashId = Path.GetFileNameWithoutExtension(file),
                        Path = path,
                        ExpiryDate = expiryDate,
                        LastModifiedDate = GetLastModified(file)
                    };
                }
            }
        }

        public void DeleteCacheFiles(IEnumerable<string> files)
        {
            files = files.Distinct();

            // kinda try to keep things under control
            var options = new ParallelOptions { MaxDegreeOfParallelism = 20 };

            Parallel.ForEach(files, options, file =>
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(file)) return;
                    if (FileExists(file) == false) return;
                    DeleteFile(file);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Failed to delete cache file '{File}'.", file);
                }
            });
        }

        protected static string GetCachePath(string hash, string extension)
        {
            var path = $"{_defaultPath}{hash}";
            if (!string.IsNullOrEmpty(extension))
            {
                path += $".{extension}";
            }
            return path;
        }

        public FileCacheData AddCacheFile(string hashId, string extension, Stream stream)
        {
            if (string.IsNullOrWhiteSpace(hashId))
            {
                throw new ArgumentException($"'{nameof(hashId)}' cannot be null or whitespace.", nameof(hashId));
            }

            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            var filePath = GetCachePath(hashId, extension);
            if (FileExists(filePath))
            {
                _logger.LogWarning("Cache file existed but it's was override. Hash: {hash}", hashId);
            }

            AddFile(filePath, stream, true);
            var expiryDate = GetExpiryDate(filePath);

            _logger.LogInformation("New cache file was add. Hash: {hashId}", hashId);

            return new FileCacheData()
            {
                HashId = hashId,
                Path = filePath,
                ExpiryDate = expiryDate,
                LastModifiedDate = GetLastModified(filePath)
            };
        }

        public DateTimeOffset GetExpiryDate(string file)
        {
            if (string.IsNullOrEmpty(file))
            {
                throw new ArgumentException($"'{nameof(file)}' cannot be null or empty.", nameof(file));
            }
            if (ExpirationTimeSpan.Ticks < 0)
            {
                var lastModified = GetLastModified(file);
                return lastModified.Add(ExpirationTimeSpan);
            }
            return _dateTimeProvider.UtcNow.Add(TimeSpan.FromDays(365));
        }

        public IEnumerable<FileCacheData> GetAllCacheFiles(string path = null)
        {
            if (string.IsNullOrEmpty(path))
                path = _defaultPath;

            var files = GetFiles(path);

            foreach (var file in files)
            {
                var expiryDate = GetExpiryDate(file);

                yield return new FileCacheData
                {
                    HashId = Path.GetFileNameWithoutExtension(file),
                    Path = file,
                    ExpiryDate = expiryDate,
                    LastModifiedDate = GetLastModified(file)
                };
            }
        }

        public IEnumerable<string> GetDirectories(string path)
        {
            return _innerFileSystem.GetDirectories(path);
        }

        public void DeleteDirectory(string path)
        {
            _innerFileSystem.DeleteDirectory(path);
        }

        public void DeleteDirectory(string path, bool recursive)
        {
            _innerFileSystem.DeleteDirectory(path, recursive);
        }

        public bool DirectoryExists(string path)
        {
            return _innerFileSystem.DirectoryExists(path);
        }

        public void AddFile(string path, Stream stream)
        {
            _innerFileSystem.AddFile(path, stream);
        }

        public void AddFile(string path, Stream stream, bool overrideIfExists)
        {
            _innerFileSystem.AddFile(path, stream, overrideIfExists);
        }

        public IEnumerable<string> GetFiles(string path)
        {
            return _innerFileSystem.GetFiles(path);
        }

        public IEnumerable<string> GetFiles(string path, string filter)
        {
            return _innerFileSystem.GetFiles(path, filter);
        }

        public Stream OpenFile(string path)
        {
            return _innerFileSystem.OpenFile(path);
        }

        public void DeleteFile(string path)
        {
            _innerFileSystem.DeleteFile(path);
        }

        public bool FileExists(string path)
        {
            return _innerFileSystem.FileExists(path);
        }

        public string GetRelativePath(string fullPathOrUrl)
        {
            return _innerFileSystem.GetRelativePath(fullPathOrUrl);
        }

        public string GetFullPath(string path)
        {
            return _innerFileSystem.GetFullPath(path);
        }

        public string GetUrl(string path)
        {
            return _innerFileSystem.GetUrl(path);
        }

        public DateTimeOffset GetLastModified(string path)
        {
            return _innerFileSystem.GetLastModified(path);
        }

        public DateTimeOffset GetCreated(string path)
        {
            return _innerFileSystem.GetCreated(path);
        }

        public long GetSize(string path)
        {
            return _innerFileSystem.GetSize(path);
        }

        public void AddFile(string path, string physicalPath, bool overrideIfExists = true, bool copy = false)
        {
            _innerFileSystem.AddFile(path, physicalPath, overrideIfExists, copy);
        }
    }
}
