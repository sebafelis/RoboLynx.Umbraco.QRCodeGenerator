using DotNetColorParser;
using LightInject;
using RoboLynx.Umbraco.QRCodeGenerator.Extensions;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;
using Umbraco.Core.IO;

namespace RoboLynx.Umbraco.QRCodeGenerator.Core
{
    public class Compositor : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {
            foreach (var type in TypeFinder.FindClassesOfType<IQRCodeType>())
            {
                serviceRegistry.Register(typeof(IQRCodeType), type, type.Name, new PerRequestLifeTime());
            }
            serviceRegistry.Register(f => new QRCodeTypesCollection(f.GetAllInstances<IQRCodeType>()), new PerRequestLifeTime());

            foreach (var source in TypeFinder.FindClassesOfType<IQRCodeSource>())
            {
                serviceRegistry.Register(typeof(IQRCodeSource), source, source.Name, new PerRequestLifeTime());
            }
            serviceRegistry.Register(f => new QRCodeSourcesCollection(f.GetAllInstances<IQRCodeSource>()), new PerRequestLifeTime());

            foreach (var format in TypeFinder.FindClassesOfType<IQRCodeFormat>())
            {
                serviceRegistry.Register(typeof(IQRCodeFormat), format, format.Name, new PerRequestLifeTime());
            }

            serviceRegistry.Register(f => new QRCodeFormatsCollection(f.GetAllInstances<IQRCodeFormat>()), new PerRequestLifeTime());

            serviceRegistry.Register<IColorNotationProvider>(f => new ColorNotationProvider(true), new PerContainerLifetime());
            serviceRegistry.Register<IColorParser, ColorParser>(new PerContainerLifetime());
            serviceRegistry.Register<IQRCodeBuilder, QRCodeBuilder>(new PerRequestLifeTime());


        }
    }
}
