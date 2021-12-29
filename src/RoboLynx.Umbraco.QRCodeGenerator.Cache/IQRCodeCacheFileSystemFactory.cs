using System;
using Umbraco.Core.IO;

namespace RoboLynx.Umbraco.QRCodeGenerator.Cache
{
    public interface IQRCodeCacheFileSystemFactory
    {
        IQRCodeCacheFileSystem CreateFileSystem(IFileSystem innerFileSystem, TimeSpan expirationTimeSpan);
    }
}
