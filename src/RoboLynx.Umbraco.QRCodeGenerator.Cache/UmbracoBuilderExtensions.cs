using Chronos.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Sync;

namespace RoboLynx.Umbraco.QRCodeGenerator.Cache
{
    public static class UmbracoBuilderExtensions
    {
        public static IUmbracoBuilder AddQRCodeCache<T>(this IUmbracoBuilder umbracoBuilder, Func<IServiceProvider, IFileSystem> fileSystem, Func<IServiceProvider, IQRCodeCacheUrlProvider> cacheUrlProvider) where T : IQRCodeCacheRole
        {
            var cacheName = Activator.CreateInstance<T>().Name;

            umbracoBuilder.Services.AddSingleton(f => new QRCodeCache<T>(umbracoBuilder.AppCaches,
                    new QRCodeCacheFileSystem(fileSystem(f), f.GetRequiredService<IOptionsMonitor<QRCodeCacheOptions>>(), 
                        f.GetRequiredService<IDateTimeOffsetProvider>(), f.GetRequiredService<ILogger<QRCodeCacheFileSystem>>()),
                    cacheUrlProvider(f),
                    f.GetRequiredService<IProfilingLogger>(),
                    f.GetRequiredService<ILogger<QRCodeCache<T>>>(),
                    f.GetRequiredService<IDateTimeOffsetProvider>(),
                    f.GetRequiredService<IServerRoleAccessor>())
            );


            umbracoBuilder.Services.AddHostedService(f => new CleanCacheHostedService(
                    cacheName,
                    f.GetRequiredService<IOptionsSnapshot<QRCodeCacheOptions>>(), 
                    f.GetRequiredService<IServerRoleAccessor>(), 
                    f.GetRequiredService<QRCodeCache<T>>(),
                    f.GetRequiredService<ILogger<CleanCacheHostedService>>())
            );

            return umbracoBuilder;
        }
    }
}
