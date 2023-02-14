using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RoboLynx.Umbraco.QRCodeGenerator.Cache;
using System.IO;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Hosting;
using Umbraco.Cms.Core.IO;

namespace RoboLynx.Umbraco.QRCodeGenerator.Frontend.Cache.Local
{
    [ComposeBefore(typeof(QRCodeGeneratorCoreComposer))]
    public class QRCodeGeneratorFrontendCacheLocalComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            var cacheLocation = FrontendCache.DefaultFrontendCacheLocation;
            builder.AddQRCodeCache<FrontendQRCodeCache>(f =>
            {
                var hostingEnvironment = f.GetRequiredService<IHostingEnvironment>();
                return new PhysicalFileSystem(f.GetRequiredService<IIOHelper>(), hostingEnvironment, f.GetRequiredService<ILogger<PhysicalFileSystem>>(),
                    Path.Join(hostingEnvironment.LocalTempPath, cacheLocation), "/");
            }, null);
        }
    }
}