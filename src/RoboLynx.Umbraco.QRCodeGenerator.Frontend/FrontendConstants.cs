namespace RoboLynx.Umbraco.QRCodeGenerator.Frontend
{
    public static class Frontend
    {
        public const string FrontendApiOptionSectionName = "FrontendApi";

        private const string _frontendCacheNameValue = "Frontend";

        public static string FrontendCacheName { get; set; } = _frontendCacheNameValue;

        internal const string CryptoParamName = "c";
    }
}