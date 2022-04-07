using Microsoft.Extensions.DependencyInjection;
using RoboLynx.Umbraco.QRCodeGenerator.Controllers;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace RoboLynx.Umbraco.QRCodeGenerator
{
    [ComposeAfter(typeof(QRCodeGeneratorCoreComposer))]
    public class QRCodeGeneratorComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            builder.Services.AddTransient<IQRCodeResponesFactory, QRCodeResponesFactory>();
            builder.Services.AddTransient<QRCodeController>();
            builder.Services.AddTransient<QRCodeFormatPickerController>();
            builder.Services.AddTransient<QRCodeLevelPickerController>();
            builder.Services.AddTransient<QRCodeSourcePickerController>();
            builder.Services.AddTransient<QRCodeTypePickerController>();

            builder.ContentApps().Append<QRCodeGeneratorApp>();
        }
    }
}