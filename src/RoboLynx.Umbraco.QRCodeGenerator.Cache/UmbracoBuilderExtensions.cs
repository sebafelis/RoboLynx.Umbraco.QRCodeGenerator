using Chronos.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Sync;

namespace RoboLynx.Umbraco.QRCodeGenerator.Cache
{
    public static class UmbracoBuilderExtensions
    {
        public static IUmbracoBuilder AddQRCodeCache<T>(this IUmbracoBuilder umbracoBuilder, Func<IServiceProvider, IFileSystem> fileSystem,
            Func<IServiceProvider, IQRCodeCacheUrlProvider> cacheUrlProvider) where T : IQRCodeCacheRole
        {
            var cacheName = Activator.CreateInstance<T>().Name;
            var configurationSectionKey = $"{Constants.Core.OptionsSectionName}:Cache:{cacheName}";
            umbracoBuilder.Services.AddOptions<QRCodeCacheOptions>(cacheName).Bind(umbracoBuilder.Config.GetSection(configurationSectionKey)).ValidateDataAnnotations();

            var options = new QRCodeCacheOptions();
            umbracoBuilder.Config.GetSection(configurationSectionKey).Bind(options);

            if (!options.Disable)
            {
                umbracoBuilder.Services.AddSingleton<IQRCodeCache>(f => new QRCodeCache<T>(umbracoBuilder.AppCaches,
                    new QRCodeCacheFileSystem(cacheName, fileSystem(f), f.GetRequiredService<IOptionsMonitor<QRCodeCacheOptions>>(),
                        f.GetRequiredService<IDateTimeOffsetProvider>(), f.GetRequiredService<ILogger<QRCodeCacheFileSystem>>()),
                    cacheUrlProvider?.Invoke(f),
                    f.GetRequiredService<IProfilingLogger>(),
                    f.GetRequiredService<ILogger<QRCodeCache<T>>>(),
                    f.GetRequiredService<IDateTimeOffsetProvider>(),
                    f.GetRequiredService<IServerRoleAccessor>())
                ).AddHostedService(f => new RecurringCleanupCache(
                    cacheName,
                    f.GetRequiredService<IOptionsMonitor<QRCodeCacheOptions>>(),
                    f.GetRequiredService<IServerRoleAccessor>(),
                    f.GetRequiredService<IQRCodeCacheManager>(),
                    f.GetRequiredService<ILogger<RecurringCleanupCache>>())
                ).AddHostedService(f => new CacheInitializeTask(
                    cacheName,
                    f.GetRequiredService<IServerRoleAccessor>(),
                    f.GetRequiredService<IQRCodeCacheManager>(),
                    f.GetRequiredService<ILogger<CacheInitializeTask>>())
                );
            }
            else
            {
                umbracoBuilder.Services.BuildServiceProvider().GetService<ILogger<QRCodeCache<T>>>().LogWarning("Cache was disable in configuration.", cacheName);
            }

            return umbracoBuilder;
        }
    }
}