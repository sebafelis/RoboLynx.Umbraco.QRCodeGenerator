﻿using DotNetColorParser;
using RoboLynx.Umbraco.QRCodeGenerator.Controllers;
using RoboLynx.Umbraco.QRCodeGenerator.Extensions;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Web;

namespace RoboLynx.Umbraco.QRCodeGenerator
{
    public class QRCodeGeneratorComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            composition.QRCodeTypes().Add(() => composition.TypeLoader.GetTypes<IQRCodeType>());

            composition.QRCodeSources().Add(() => composition.TypeLoader.GetTypes<IQRCodeSource>());

            composition.QRCodeFormats().Add(() => composition.TypeLoader.GetTypes<IQRCodeFormat>());

            composition.Register<IColorNotationProvider>(f => new ColorNotationProvider(true), Lifetime.Singleton);
            composition.Register<IColorParser, ColorParser>(Lifetime.Singleton);
            composition.Register<IQRCodeBuilder, QRCodeBuilder>(Lifetime.Request);

            composition.Register<QRCodeController>(Lifetime.Request);
            composition.Register<QRCodeFormatPickerController>(Lifetime.Request);
            composition.Register<QRCodeLevelPickerController>(Lifetime.Request);
            composition.Register<QRCodeSourcePickerController>(Lifetime.Request);
            composition.Register<QRCodeTypePickerController>(Lifetime.Request);

            composition.ContentApps().Append<QRCodeGeneratorApp>();
        }
    }
}
