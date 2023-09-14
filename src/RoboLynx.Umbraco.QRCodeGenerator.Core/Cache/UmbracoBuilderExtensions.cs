using Chronos.Abstractions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Sync;
using Umbraco.Extensions;

namespace RoboLynx.Umbraco.QRCodeGenerator.Cache
{
    public static class UmbracoBuilderExtensions
    {

        public static IUmbracoBuilder AddLocalQRCodeCache<TCacheRole>(this IUmbracoBuilder builder, Action<QRCodeCacheOptions>? configure = null) where TCacheRole : class, IQRCodeCacheRole
        {
            builder.Services.AddSingleton<TCacheRole>();

            var cacheRole = builder.Services.BuildServiceProvider().GetRequiredService<TCacheRole>();

            ApplyConfiguration(builder, cacheRole, configure != null ? builder => builder.Configure(configure) : null);

            var configuration = GetConfiguration(builder, cacheRole);

            var baseUrl = "/";

            Func<IServiceProvider, IQRCodeCacheUrlProvider>? provider = null;

            if (!string.IsNullOrWhiteSpace(configuration.CacheBaseUrl) && configuration.CacheBaseUrl != "/")
            {
                baseUrl = configuration.CacheBaseUrl;
                provider = f => f.CreateInstance<LocalCacheUrlProvider>(baseUrl);
            }

            CreateQRCodeCache(builder, cacheRole, f =>
            {
                var cacheLocation = configuration.CacheLocation;
                if (string.IsNullOrEmpty(cacheLocation))
                {
                    cacheLocation = cacheRole.DefaultLocation;
                }
                return f.CreateInstance<PhysicalFileSystem>(GetCacheLocation(cacheLocation, f.GetRequiredService<IWebHostEnvironment>()), baseUrl);
            }, provider);

            return builder;
        }

        public static IUmbracoBuilder AddQRCodeCache<TCacheRole>(this IUmbracoBuilder builder, Func<IServiceProvider, IFileSystem> fileSystem,
            Func<IServiceProvider, IQRCodeCacheUrlProvider>? cacheUrlProvider, Action<QRCodeCacheOptions>? configure = null) where TCacheRole : class, IQRCodeCacheRole
        {
            builder.Services.AddSingleton<TCacheRole>();

            var cacheRole = builder.Services.BuildServiceProvider().GetRequiredService<TCacheRole>();

            ApplyConfiguration(builder, cacheRole, configure != null ? builder => builder.Configure(configure) : null);

            CreateQRCodeCache(builder, cacheRole, fileSystem, cacheUrlProvider);

            return builder;
        }

        private static void CreateQRCodeCache<TCacheRole>(IUmbracoBuilder builder, TCacheRole cacheRole, Func<IServiceProvider, IFileSystem> fileSystem,
            Func<IServiceProvider, IQRCodeCacheUrlProvider>? cacheUrlProvider) where TCacheRole : class, IQRCodeCacheRole
        {
            var cacheName = cacheRole.Name;

            //builder.Services.AddSingleton(cacheRole);

            //builder.Services.AddSingleton<IQRCodeCache>(f => f.CreateInstance<QRCodeCache<TCacheRole>>(f.CreateInstance<QRCodeCacheFileSystem>(cacheName, fileSystem(f)), cacheUrlProvider, cacheRole));

            builder.Services.AddSingleton<IQRCodeCache>(f => new QRCodeCache<TCacheRole>(builder.AppCaches, f.CreateInstance<QRCodeCacheFileSystem>(cacheName, fileSystem.Invoke(f)), cacheUrlProvider?.Invoke(f), cacheRole,
                f.GetRequiredService<IProfilingLogger>(), f.GetRequiredService<ILogger<QRCodeCache<TCacheRole>>>(), f.GetRequiredService<IDateTimeOffsetProvider>(),
                f.GetRequiredService<IServerRoleAccessor>()));

            builder.Services.AddHostedService(f => f.CreateInstance<RecurringCleanupCache>(cacheName))
                .AddHostedService(f => f.CreateInstance<CacheInitializeTask>(cacheName));
        }

        private static string GetCacheLocation(string cacheLocation, IWebHostEnvironment webHostEnvironment)
        {
            if (!Path.IsPathFullyQualified(cacheLocation))
            {
                cacheLocation = webHostEnvironment.MapPathWebRoot(cacheLocation);
            }
            if (Path.EndsInDirectorySeparator(cacheLocation))
            {
                cacheLocation = Path.TrimEndingDirectorySeparator(cacheLocation);
            }
            return cacheLocation;
        }


        private static void ApplyConfiguration(IUmbracoBuilder builder, IQRCodeCacheRole cacheRole, Action<OptionsBuilder<QRCodeCacheOptions>>? configure = null)
        {
            var configurationSectionKey = $"{Core.OptionsSectionName}:Cache:{cacheRole.Name}";

            OptionsBuilder<QRCodeCacheOptions> optionsBuilder = builder.Services.AddOptions<QRCodeCacheOptions>(cacheRole.Name);

            configure?.Invoke(optionsBuilder);

            optionsBuilder
                .BindConfiguration(configurationSectionKey)
                .PostConfigure(options =>
                {
                    if (string.IsNullOrEmpty(options.CacheLocation))
                    {
                        options.CacheLocation = cacheRole.DefaultLocation;
                    }
                    if (string.IsNullOrEmpty(options.CacheBaseUrl))
                    {
                        options.CacheBaseUrl = "/";
                    }
                })
                .ValidateDataAnnotations();
        }

        private static QRCodeCacheOptions GetConfiguration(IUmbracoBuilder builder, IQRCodeCacheRole cacheRole)
        {
            var options = builder.Services.BuildServiceProvider().GetRequiredService<IOptionsMonitor<QRCodeCacheOptions>>().Get(cacheRole.Name);

            return options;
        }
    }
}