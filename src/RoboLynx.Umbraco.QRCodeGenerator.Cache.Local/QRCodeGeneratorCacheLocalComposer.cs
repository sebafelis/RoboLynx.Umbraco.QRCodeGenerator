using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Hosting;
using Umbraco.Cms.Core.IO;

namespace RoboLynx.Umbraco.QRCodeGenerator.Cache.Local
{
    [ComposeBefore(typeof(QRCodeGeneratorCoreComposer))]
    public class QRCodeGeneratorCacheLocalComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            //builder.Services.Configure<QRCodeCacheOptions>(Constants.BackofficeCacheName, builder.Config.GetSection($"{QRCodeGenerator.Constants.OptionsSectionName}:Cache:{Constants.BackofficeCacheName}")
            //builder.Services.AddOptions<QRCodeCacheOptions>(Constants.BackofficeCacheName).Bind(builder.Config.GetSection($"{QRCodeGenerator.Constants.OptionsSectionName}:Cache:{Constants.BackofficeCacheName}"));

            var options = new QRCodeCacheOptions();
            builder.Config.GetSection($"{Constants.Core.OptionsSectionName}:Cache:{Constants.Backoffice.BackofficeCacheName}").Bind(Constants.Backoffice.BackofficeCacheName, options);

            if (!options.Disable)
            {
                builder.AddQRCodeCache<BackofficeQRCodeCache>(f => new PhysicalFileSystem(f.GetService<IIOHelper>(), f.GetService<IHostingEnvironment>(), f.GetService<ILogger<PhysicalFileSystem>>(), options.CacheLocation, string.Empty), null);
            }
        }
    }
}
