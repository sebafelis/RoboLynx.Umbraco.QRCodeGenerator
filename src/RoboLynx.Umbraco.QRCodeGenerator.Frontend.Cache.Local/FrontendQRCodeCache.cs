using RoboLynx.Umbraco.QRCodeGenerator.Cache;

namespace RoboLynx.Umbraco.QRCodeGenerator.Frontend.Cache.Local
{
    public class FrontendQRCodeCache : IQRCodeCacheRole
    {
        public static string CacheName => Frontend.FrontendCacheName;

        public string Name => CacheName;
    }
}