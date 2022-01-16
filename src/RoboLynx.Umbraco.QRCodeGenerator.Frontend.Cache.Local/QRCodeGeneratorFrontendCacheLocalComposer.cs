using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RoboLynx.Umbraco.QRCodeGenerator.Cache;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Hosting;
using Umbraco.Cms.Core.IO;

namespace RoboLynx.Umbraco.QRCodeGenerator.Frontend.Cache
{
    [ComposeBefore(typeof(QRCodeGeneratorCoreComposer))]
    public class QRCodeGeneratorFrontendCacheLocalComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            var options = new QRCodeCacheOptions();
            builder.Config.GetSection($"{Constants.Core.OptionsSectionName}:Cache:{Constants.Frontend.FrontendCacheName}").Bind(Constants.Frontend.FrontendCacheName, options);

            if (!options.Disable)
            {
                builder.AddQRCodeCache<FrontendQRCodeCache>(f => new PhysicalFileSystem(f.GetService<IIOHelper>(), f.GetService<IHostingEnvironment>(), f.GetService<ILogger<PhysicalFileSystem>>(), options.CacheLocation, string.Empty), null);
            }
        }
    }
}
