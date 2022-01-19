using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RoboLynx.Umbraco.QRCodeGenerator.Cache;
using System.IO;
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
            var cacheLocation = Constants.FrontendCache.DefaultFrontendCacheLocation;
            builder.AddQRCodeCache<FrontendQRCodeCache>(f => {
                var hostingEnvironment = f.GetService<IHostingEnvironment>();
                return new PhysicalFileSystem(f.GetService<IIOHelper>(), hostingEnvironment, f.GetService<ILogger<PhysicalFileSystem>>(),
                    Path.Join(hostingEnvironment.LocalTempPath, cacheLocation), "/"); 
            }, null);
        }
    }
}
