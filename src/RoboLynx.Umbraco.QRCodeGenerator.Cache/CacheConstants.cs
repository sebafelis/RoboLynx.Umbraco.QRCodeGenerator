namespace RoboLynx.Umbraco.QRCodeGenerator.Cache
{
    public static class Cache
    {
        public static string DefaultLocalCacheLocation => $"QRCodeGeneratorCache\\{Backoffice.BackofficeCacheName}";
    }
}