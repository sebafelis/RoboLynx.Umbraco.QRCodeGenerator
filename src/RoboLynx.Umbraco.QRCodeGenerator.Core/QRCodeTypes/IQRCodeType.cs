using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QRCoder.PayloadGenerator;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes
{
    public interface IQRCodeType
    {
        string Name { get; }
        string Description { get; }
        string Value { get; }
    }
}
