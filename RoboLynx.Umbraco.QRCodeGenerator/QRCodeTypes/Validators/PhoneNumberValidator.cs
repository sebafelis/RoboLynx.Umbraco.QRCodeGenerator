using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes.Validators
{
    public class PhoneNumberValidator : IQRCodeTypeValidator
    {
        public bool Validate(object value, out string message)
        {
            if (value is string)
            {
                var isValid = Regex.IsMatch((string)value, @"^\+(9[976]\d|8[987530]\d|6[987]\d|5[90]\d|42\d|3[875]\d|2[98654321]\d|9[8543210]|8[6421]|6[6543210]|5[87654321]|4[987654310]|3[9643210]|2[70]|7|1)\d{1,14}$");
                
                message = !isValid ? "A number is not in correct" : null;
                
                return isValid;
            }

            message = "Value is not a string";

            return false;
        }
    }
}
