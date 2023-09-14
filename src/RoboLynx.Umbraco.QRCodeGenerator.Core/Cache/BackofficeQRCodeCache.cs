using System.IO;
using Umbraco.Cms.Core.Hosting;

namespace RoboLynx.Umbraco.QRCodeGenerator.Cache
{
    public class BackofficeQRCodeCache : IQRCodeCacheRole
    {
        public static string CacheName => Backoffice.BackofficeCacheName;

        public static string RelativeCacheLocation => $"{Path.DirectorySeparatorChar}QRCodeGeneratorCache{Path.DirectorySeparatorChar}{Backoffice.BackofficeCacheName}";

        public string Name => CacheName;

        public string DefaultLocation { get; private set; }

        public BackofficeQRCodeCache(IHostingEnvironment hostingEnvironment)
        {
            DefaultLocation = Path.Join(hostingEnvironment.LocalTempPath, RelativeCacheLocation);
        }
    }
}