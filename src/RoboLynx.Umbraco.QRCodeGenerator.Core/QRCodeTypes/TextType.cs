using RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.Services;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes
{
    public class TextType : QRCodeType
    {
        const string textArgumentName = "text";

        public TextType(ILocalizedTextService localizedTextService) : base(localizedTextService)
        {

        }

        public override string Id => "Text";

        public override string Value(IQRCodeSource source, string sourceSettings, IPublishedContent content, string culture)
        {
            return source.GetValue<string>(0, textArgumentName, content, sourceSettings, culture);
        }
    }
}
