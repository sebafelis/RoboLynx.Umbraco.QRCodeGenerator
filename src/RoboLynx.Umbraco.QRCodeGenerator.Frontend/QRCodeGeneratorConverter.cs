using System;
using Umbraco.Core;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.PropertyEditors;
using Umbraco.Core.Composing;

namespace RoboLynx.Umbraco.QRCodeGenerator.Frontend
{
    public class QRCodeGeneratorConverter : PropertyValueConverterBase
    {
        private readonly IFactory _factory;

        public QRCodeGeneratorConverter(IFactory factory)
        {
            _factory = factory;
        }

        public override object ConvertSourceToIntermediate(IPublishedElement owner, IPublishedPropertyType propertyType, object source, bool preview)
        {
            if (owner is IPublishedContent content)
            {
                var qrCodeService = _factory.GetInstance<IQRCodeService>();

                return new QRCode(qrCodeService, content, propertyType.Alias, Constants.FrontendCacheName);
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
            return propertyType.EditorAlias.Equals(RoboLynx.Umbraco.QRCodeGenerator.Constants.PropertyEditorAlias);
        }
    }
}
