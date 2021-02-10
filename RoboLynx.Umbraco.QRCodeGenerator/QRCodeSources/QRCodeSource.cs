using System;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.Services;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources
{
    public abstract class QRCodeSource : IQRCodeSource
    {
        protected readonly ILocalizedTextService textService;

        public QRCodeSource(ILocalizedTextService textService)
        {
            this.textService = textService;
        }

        public abstract string Id { get; }
        public string Name => textService.Localize($"qrCodeSources/{GetType().Name.ToFirstLower()}Name") ?? GetType().Name;
        public string Description => textService.Localize($"qrCodeSources/{GetType().Name.ToFirstLower()}Description") ?? string.Empty;

        public abstract T GetValue<T>(int index, string key, IPublishedContent content, string sourceSettings) where T : IConvertible;
    }
}
