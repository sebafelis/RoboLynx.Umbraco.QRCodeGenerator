using System.IO;
using Umbraco.Cms.Core.Hosting;

namespace RoboLynx.Umbraco.QRCodeGenerator.Cache
{
    public class FrontendQRCodeCache : IQRCodeCacheRole
    {
        public static string CacheName => Frontend.Frontend.FrontendCacheName;

        public static string RelativeCacheLocation => $"{Path.DirectorySeparatorChar}QRCodeGeneratorCache{Path.DirectorySeparatorChar}{Frontend.Frontend.FrontendCacheName}";

        public string Name => CacheName;

        public string DefaultLocation { get; private set; }

        public FrontendQRCodeCache(IHostingEnvironment hostingEnvironment)
        {
            DefaultLocation = Path.Join(hostingEnvironment.LocalTempPath, RelativeCacheLocation);
        }
    }
}