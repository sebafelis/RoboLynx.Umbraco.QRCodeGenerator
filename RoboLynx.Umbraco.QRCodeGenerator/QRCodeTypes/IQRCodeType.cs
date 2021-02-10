using RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Composing;
using Umbraco.Core.Models.PublishedContent;
using static QRCoder.PayloadGenerator;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes
{
    public interface IQRCodeType : IDiscoverable
    {
        string Id { get; }
        string Name { get; }
        string Description { get; }
        string Value(IQRCodeSource source, string sourceSettings, IPublishedContent content);
    }
}
