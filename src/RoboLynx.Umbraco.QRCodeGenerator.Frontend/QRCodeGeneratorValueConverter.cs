using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Routing;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.PropertyEditors;

namespace RoboLynx.Umbraco.QRCodeGenerator.Frontend
{
    public class QRCodeGeneratorValueConverter : PropertyValueConverterBase
    {
        private readonly UrlHelper _urlHelper;
        private readonly IQRCodeBuilder _qrCodeBuilder;

        public QRCodeGeneratorValueConverter(HttpRequestMessage httpRequestMessage, IQRCodeBuilder qrCodeBuilder)
        {
            _urlHelper = new UrlHelper(httpRequestMessage);
            _qrCodeBuilder = qrCodeBuilder ?? throw new ArgumentNullException(nameof(qrCodeBuilder));
        }

        public override object ConvertSourceToIntermediate(IPublishedElement owner, IPublishedPropertyType propertyType, object source, bool preview)
        {
            if (owner is IPublishedContent content)
            {
                return new QRCodeProperty(_urlHelper, content, propertyType.Alias, _qrCodeBuilder);
            }
            return null;
        }

        public override PropertyCacheLevel GetPropertyCacheLevel(IPublishedPropertyType propertyType)
        {
            return PropertyCacheLevel.Elements;
        }

        public override Type GetPropertyValueType(IPublishedPropertyType propertyType)
        {
            return typeof(QRCodeProperty);
        }

        public override bool IsConverter(IPublishedPropertyType propertyType)
        {
            return propertyType.EditorAlias.Equals(RoboLynx.Umbraco.QRCodeGenerator.Constants.PropertyEditorAlias);
        }
    }
}
