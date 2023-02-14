namespace RoboLynx.Umbraco.QRCodeGenerator.Frontend.Cache.Local
{
    internal static class FrontendCache
    {
        public static string DefaultFrontendCacheLocation => $"QRCodeGeneratorCache\\{Frontend.FrontendCacheName}";
    }
}