using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RoboLynx.Umbraco.QRCodeGenerator.Frontend.Controllers;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace RoboLynx.Umbraco.QRCodeGenerator.Frontend
{
    [ComposeAfter(typeof(QRCodeGeneratorComposer))]
    public class QRCodeGeneratorFrontendComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            var options = new QRCodeFrontendOptions();
            builder.Config.GetSection($"{Constants.Core.OptionsSectionName}:{Constants.Frontend.FrontendApiOptionSectionName}").Bind(options);

            if (!options.Disable)
            {
                builder.Services.AddTransient<PublicQRCodeController>();
            }
        }
    }
}