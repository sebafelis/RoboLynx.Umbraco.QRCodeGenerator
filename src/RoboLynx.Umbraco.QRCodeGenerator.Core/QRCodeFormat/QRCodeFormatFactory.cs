using RoboLynx.Umbraco.QRCodeGenerator.Models;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes;
using System.Collections.Generic;
using Umbraco.Core;
using Umbraco.Core.Services;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat
{
    public abstract class QRCodeFormatFactory : IQRCodeFormatFactory
    {
        protected ILocalizedTextService LocalizedTextService { get; }

        protected struct QRCodeConfigContainer
        {
            public string CodeContent { get; }
            public QRCodeSettings Settings { get; }

            public QRCodeConfigContainer(string codeContent, QRCodeSettings settings) : this()
            {
                CodeContent = codeContent;
                Settings = settings;
            }
        }

        public QRCodeFormatFactory(ILocalizedTextService localizedTextService)
        {
            LocalizedTextService = localizedTextService;
        }

        public abstract string Id { get; }

        public virtual string Name => LocalizedTextService.Localize($"qrCodeFormats/{GetType().Name.Replace("Factory", "").ToFirstLower()}Name");

        public abstract IEnumerable<string> RequiredSettings { get; }

        public abstract IQRCodeFormat Create(IQRCodeType codeType, QRCodeSettings settings);
    }
}
