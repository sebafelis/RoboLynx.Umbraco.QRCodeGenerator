namespace RoboLynx.Umbraco.QRCodeGenerator.Cache
{
    public class BackofficeQRCodeCache : IQRCodeCacheRole
    {
        public static string CacheName => Backoffice.BackofficeCacheName;

        public string Name => CacheName;
    }
}