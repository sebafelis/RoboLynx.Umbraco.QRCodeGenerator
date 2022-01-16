using System;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;

namespace RoboLynx.Umbraco.QRCodeGenerator.Frontend
{
    public class QRCodeGeneratorConverter : PropertyValueConverterBase
    {
        private readonly IQRCodeService _qRCodeService;

        public QRCodeGeneratorConverter(IQRCodeService qRCodeService)
        {
            _qRCodeService = qRCodeService;
        }

        public override object ConvertSourceToIntermediate(IPublishedElement owner, IPublishedPropertyType propertyType, object source, bool preview)
        {
            if (owner is IPublishedContent content)
            {
                return new QRCode(_qRCodeService, content, propertyType.Alias, Constants.Frontend.FrontendCacheName);
            }
            return null;
        }

        public override PropertyCacheLevel GetPropertyCacheLevel(IPublishedPropertyType propertyType)
        {
            return PropertyCacheLevel.Elements;
        }

        public override Type GetPropertyValueType(IPublishedPropertyType propertyType)
        {
            return typeof(QRCode);
        }

        public override bool IsConverter(IPublishedPropertyType propertyType)
        {
            return propertyType.EditorAlias.Equals(Constants.Backoffice.PropertyEditorAlias);
        }
    }
}
