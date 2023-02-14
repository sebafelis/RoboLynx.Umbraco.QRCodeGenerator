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
            var configSection = builder.Config.GetSection($"{Core.OptionsSectionName}:{Frontend.FrontendApiOptionSectionName}");
            builder.Services.Configure<QRCodeFrontendOptions>(configSection);

            var options = configSection.Get<QRCodeFrontendOptions>() ?? new QRCodeFrontendOptions();

            if (!options.Disable)
            {
                builder.Services.AddSingleton<IQueryCipher, QueryCipher>();
                builder.Services.AddTransient<PublicQRCodeController>();
                UrlHelperExtensionsForQRCodeGenerator.QueryCipher = builder.Services.BuildServiceProvider().GetService<IQueryCipher>();
            }
        }
    }
}