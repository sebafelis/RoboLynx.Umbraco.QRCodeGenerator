namespace RoboLynx.Umbraco.QRCodeGenerator.Cache
{
    public interface IQRCodeCacheRole
    {
        public string Name { get; }

        public string DefaultLocation { get; }
    }
}