using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IO;
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
            var cacheLocation = Constants.Cache.DefaultLocalCacheLocation;

            builder.AddQRCodeCache<BackofficeQRCodeCache>(f => {
                var hostingEnvironment = f.GetService<IHostingEnvironment>();
                return new PhysicalFileSystem(f.GetService<IIOHelper>(), hostingEnvironment, f.GetService<ILogger<PhysicalFileSystem>>(),
                   Path.Join(hostingEnvironment.LocalTempPath, cacheLocation), "/");
            }, null);
        }
    }
}
