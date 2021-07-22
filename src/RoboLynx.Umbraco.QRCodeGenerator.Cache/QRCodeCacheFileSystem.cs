using Chronos.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;
using Umbraco.Core.IO;
using Umbraco.Core.Logging;

namespace RoboLynx.Umbraco.QRCodeGenerator.Cache
{
    public class QRCodeCacheFileSystem : FileSystemWrapper, IQRCodeCacheFileSystem
    {
        const string _defaultPath = "";

        private readonly ILogger _logger;
        private readonly IDateTimeOffsetProvider _dateTimeProvider;

        public QRCodeCacheFileSystem(IFileSystem innerFileSystem, TimeSpan expirationTimeSpan, ILogger logger, IDateTimeOffsetProvider dateTimeProvider) : base(innerFileSystem)
        {
            ExpirationTimeSpan = expirationTimeSpan;
            this._logger = logger;
            this._dateTimeProvider = dateTimeProvider;
        }

        public TimeSpan ExpirationTimeSpan { get; }

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
                        ExpiryDate = expiryDate
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
                    if (file.IsNullOrWhiteSpace()) return;
                    if (FileExists(file) == false) return;
                    DeleteFile(file);
                }
                catch (Exception e)
                {
                    _logger.Error<QRCodeCacheFileSystem>(e, "Failed to delete cache file '{File}'.", file);
                }
            });
        }

        protected string GetCachePath(string hash, string extension)
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
                _logger.Warn<QRCodeCacheFileSystem>("Cache file existed but it's was override. Hash: {hash}", hashId);
            }

            AddFile(filePath, stream, true);
            var expiryDate = GetExpiryDate(filePath);

            _logger.Info<QRCodeCacheFileSystem>("New cache file was add. Hash: {hashId}", hashId);

            return new FileCacheData()
            {
                HashId = hashId,
                Path = filePath,
                ExpiryDate = expiryDate
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
                    ExpiryDate = expiryDate
                };
            }
        }
    }
}
