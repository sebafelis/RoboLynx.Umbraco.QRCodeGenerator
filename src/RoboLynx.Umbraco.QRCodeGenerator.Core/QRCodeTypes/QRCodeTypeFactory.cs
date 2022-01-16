using RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources;
using System;
using Umbraco.Cms.Core.Services;
using Umbraco.Extensions;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes
{
    public abstract class QRCodeTypeFactory : IQRCodeTypeFactory
    {
        protected readonly ILocalizedTextService localizedTextService;

        public QRCodeTypeFactory(ILocalizedTextService localizedTextService)
        {
            this.localizedTextService = localizedTextService;
        }

        public abstract string Id { get; }

        public virtual string Name => localizedTextService == null ? throw new NotSupportedException("LocalizedTextService was not passed in constructor.") : (localizedTextService.Localize("qrCodeTypes", $"{GetType().Name.Replace("Factory","").ToFirstLower()}Name") ?? GetType().Name);

        public virtual string Description => localizedTextService == null ? throw new NotSupportedException("LocalizedTextService was not passed in constructor.") : (localizedTextService.Localize("qrCodeTypes", $"{GetType().Name.Replace("Factory", "").ToFirstLower()}Description") ?? string.Empty);

        public abstract IQRCodeType Create(IQRCodeSource qrCodeSource);
    }
}
