using System;
using Umbraco.Core;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.Services;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources
{
    public abstract class QRCodeSourceFactory : IQRCodeSourceFactory
    {
        private readonly ILocalizedTextService _localizedTextService;

        public QRCodeSourceFactory(ILocalizedTextService localizedTextService)
        {
            _localizedTextService = localizedTextService;
        }

        public abstract string Id { get; }

        public virtual string Name => _localizedTextService.Localize($"qrCodeSources/{GetType().Name.Replace("Factory", "").ToFirstLower()}Name") ?? GetType().Name;

        public virtual string Description => _localizedTextService.Localize($"qrCodeSources/{GetType().Name.Replace("Factory", "").ToFirstLower()}Description") ?? string.Empty;

        public abstract IQRCodeSource Create(IPublishedContent publishedContent, string sourceSettings, string culture);
    }
}
