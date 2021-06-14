using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes
{
    public interface IQRCodeTypeValueFactory
    {
        /// <summary>
        /// Get value of QR Code content by using value passed into constructor.
        /// </summary>
        /// <param name="validate">Validate passed value.</param>
        /// <returns>Code content</returns>
        /// <exception cref="RoboLynx.Umbraco.QRCodeGenerator.Exceptions.ValidationQRCodeGeneratorException">Throw when value passed in constructor is invalid.</exception>
        string Value(bool validate);
    }
}
