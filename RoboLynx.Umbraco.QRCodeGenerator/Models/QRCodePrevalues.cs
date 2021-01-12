using RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes;

namespace RoboLynx.Umbraco.QRCodeGenerator.Controllers
{
    public class QRCodePrevalues
    {
        public IQRCodeSource Source { get; set; }
        public IQRCodeType Type { get; set; }
        public QRCodeSettings DefaultSettings { get; set; }
    }
}
