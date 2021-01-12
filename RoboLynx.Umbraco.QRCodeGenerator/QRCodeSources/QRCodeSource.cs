using System;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources
{
    public abstract class QRCodeSource : IQRCodeSource
    {
        protected readonly IPublishedContent content;

        public QRCodeSource(IPublishedContent content)
        {
            this.content = content;
        }

        public string Name => ApplicationContext.Current.Services.TextService.Localize($"qrCodeSources/{GetType().Name}Name") ?? GetType().Name;
        public string Description => ApplicationContext.Current.Services.TextService.Localize($"qrCodeSources/{GetType().Name}Description") ?? string.Empty;

        public abstract T GetValue<T>(int index, string key) where T : IConvertible;

        protected void CheckContent()
        {
            if (content is null)
            {
                throw new ArgumentNullException(nameof(content));
            }
        }
    }
}
