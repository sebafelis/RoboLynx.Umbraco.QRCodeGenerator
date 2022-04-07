using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources
{
    public class AbsoluteUrlSourceFactory : QRCodeSourceFactory
    {
        private readonly IPublishedUrlProvider _publishedUrlProvider;

        public AbsoluteUrlSourceFactory(ILocalizedTextService localizedTextService, IPublishedUrlProvider publishedUrlProvider) : base(localizedTextService)
        {
            _publishedUrlProvider = publishedUrlProvider;
        }

        public override string Id => "AbsoluteUrl";

        public override IQRCodeSource Create(IPublishedContent publishedContent, string sourceSettings, string culture)
        {
            return new AbsoluteUrlSource(_publishedUrlProvider, publishedContent, culture);
        }
    }
}