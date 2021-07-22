using Umbraco.Core.Models.PublishedContent;

namespace RoboLynx.Umbraco.QRCodeGenerator.Cache
{
    public interface IQRCodeCacheUrlProvider
    {
        string Url(string path, UrlMode urlMode);
    }
}
