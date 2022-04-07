using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Core.Services;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources
{
    public class SimplePropertySourceFactory : QRCodeSourceFactory
    {
        private readonly IBackOfficeSecurityAccessor _backOfficeSecurityAccessor;
        private readonly IPublishedValueFallback _publishedValueFallback;

        public SimplePropertySourceFactory(IBackOfficeSecurityAccessor backOfficeSecurityAccessor, ILocalizedTextService localizedTextService, IPublishedValueFallback publishedValueFallback) : base(localizedTextService)
        {
            _backOfficeSecurityAccessor = backOfficeSecurityAccessor;
            _publishedValueFallback = publishedValueFallback;
        }

        public override string Id => "SilmpleProperty";

        public override IQRCodeSource Create(IPublishedContent publishedContent, string sourceSettings, string culture)
        {
            return new SimplePropertySource(_backOfficeSecurityAccessor, _publishedValueFallback, publishedContent, sourceSettings, culture);
        }
    }
}