using RoboLynx.Umbraco.QRCodeGenerator.Exceptions;
using RoboLynx.Umbraco.QRCodeGenerator.Helpers;
using RoboLynx.Umbraco.QRCodeGenerator.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Umbraco.Core;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat
{
    public abstract class QRCodeFormatFactory : IQRCodeFormatFactory
    {
        protected ILocalizedTextService LocalizedTextService { get; }
        protected UmbracoHelper UmbracoHelper { get; }

        protected struct QRCodeConfigContainer
        {
            string CodeContent { get; }
            QRCodeSettings Settings { get; }

            public QRCodeConfigContainer(string codeContent, QRCodeSettings settings) : this()
            {
                CodeContent = codeContent;
                Settings = settings;
            }
        }

        public QRCodeFormatFactory(ILocalizedTextService localizedTextService, UmbracoHelper umbracoHelper)
        {
            LocalizedTextService = localizedTextService;
            UmbracoHelper = umbracoHelper;
        }

        public abstract string Id { get; }

        public virtual string Name => LocalizedTextService.Localize($"qrCodeFormats/{GetType().Name.ToFirstLower()}Name");

        public abstract IEnumerable<string> RequiredSettings { get; }

        public abstract IQRCodeFormat Create(string codeContent, QRCodeSettings settings);
    }
}
