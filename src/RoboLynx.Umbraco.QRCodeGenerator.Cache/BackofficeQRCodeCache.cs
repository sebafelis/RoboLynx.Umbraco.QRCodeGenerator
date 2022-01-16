namespace RoboLynx.Umbraco.QRCodeGenerator.Cache
{
    public class BackofficeQRCodeCache : IQRCodeCacheRole
    {
        public static string CacheName => Constants.Backoffice.BackofficeCacheName;

        public string Name => CacheName;
    }
}
