using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes.Validators
{
    public class LenghtValidation : IQRCodeTypeValidator
    {
        public QRCodeInputType? InputType { get; private set; }

        public enum QRCodeInputType { Numeric, Alphanumeric, Binary, Kanji }

        public LenghtValidation()
        {

        }

        public LenghtValidation(QRCodeInputType inputType)
        {
            InputType = inputType;
        }

        bool IQRCodeTypeValidator.Validate(object value, out string message)
        {
            if (!InputType.HasValue)
            {
                InputType = GetInputType(InputType);
            }

            if (!InputType.HasValue)
            {
                message = "Unsupported input.";
                return false;
            }

            var maxLenght = GetMaxLenght(InputType.Value);

            var lenght = 0;

            switch (InputType)
            {
                case QRCodeInputType.Alphanumeric:
                case QRCodeInputType.Numeric:
                case QRCodeInputType.Kanji:
                    lenght = value.ToString().Length;
                    break;
                case QRCodeInputType.Binary:
                    lenght = (value as byte[]).Length;
                    break;
            }

            if (lenght > maxLenght)
            {
                message = $"Code can not contains more then {maxLenght} {InputType.Value} characters for.";
                return false;
            }

            message = null;
            return true;
        }

        private int GetMaxLenght(QRCodeInputType inputType)
        {
            switch (inputType)
            {
                case QRCodeInputType.Numeric:
                    return 7089;
                case QRCodeInputType.Binary:
                    return 2953;
                case QRCodeInputType.Kanji:
                    return 1817;
                case QRCodeInputType.Alphanumeric:
                default:
                    return 4296;
            }
        }

        private QRCodeInputType? GetInputType(object value)
        {
            switch (value)
            {
                case string strValue:
                    return QRCodeInputType.Alphanumeric;
                case sbyte sbyteValue:
                case byte byteValue:
                case short shortValue:
                case ushort ushortValue:
                case int intValue:
                case uint uintValue:
                case long longValue:
                case ulong ulongValue:
                case nint nintValue:
                case nuint nuintValue:
                case double doubleValue:
                case float floatValue:
                case decimal decimalValue:
                    return QRCodeInputType.Numeric;
                case byte[] strValue:
                    return QRCodeInputType.Binary;
                default:
                    return null;
            }
        }
    }
}
