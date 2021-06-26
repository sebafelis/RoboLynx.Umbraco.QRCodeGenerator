using RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes;
using Umbraco.Core.Models.PublishedContent;

namespace RoboLynx.Umbraco.QRCodeGenerator.Models
{
    public class QRCodeConfig
    {
        public IQRCodeSource Source { get; set; }
        public IQRCodeType Type { get; set; }
        public IQRCodeFormat Format { get; set; }
        public QRCodeSettings Settings { get; set; }
    }
}
