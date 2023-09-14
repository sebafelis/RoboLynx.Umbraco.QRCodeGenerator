using System;
using System.Collections.Generic;
using System.IO;
using Umbraco.Cms.Core.IO;

namespace RoboLynx.Umbraco.QRCodeGenerator.Cache
{
    public interface IQRCodeCacheFileSystem : IFileSystem
    {
        TimeSpan ExpirationTimeSpan { get; }

        FileCacheData AddCacheFile(string hashId, string? extension, Stream stream);

        IEnumerable<FileCacheData> GetExpiredCacheFiles(string? path = null);

        IEnumerable<FileCacheData> GetAllCacheFiles(string? path = null);

        void DeleteCacheFiles(IEnumerable<string> files);

        DateTimeOffset GetExpiryDate(string file);
    }
}