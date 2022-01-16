using RoboLynx.Umbraco.QRCodeGenerator.Cache;

namespace RoboLynx.Umbraco.QRCodeGenerator.Frontend.Cache
{
    public class FrontendQRCodeCache : IQRCodeCacheRole
    {
        public static string CacheName => Constants.Frontend.FrontendCacheName;

        public string Name => CacheName;
    }
}
