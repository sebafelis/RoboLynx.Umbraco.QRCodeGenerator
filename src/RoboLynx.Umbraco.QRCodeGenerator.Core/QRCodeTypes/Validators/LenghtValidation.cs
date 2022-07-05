namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes.Validators
{
    public class LenghtValidation : IQRCodeTypeValidator
    {
        public QRCodeInputType? InputType { get; private set; }

        public enum QRCodeInputType
        { Numeric, Alphanumeric, Binary, Kanji }

        public LenghtValidation()
        {
        }

        public LenghtValidation(QRCodeInputType inputType)
        {
            InputType = inputType;
        }

        bool IQRCodeTypeValidator.Validate(object? value, out string? message)
        {
            if (!InputType.HasValue)
            {
                InputType = GetInputType(value);
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
                    lenght = value?.ToString()?.Length ?? 0;
                    break;

                case QRCodeInputType.Binary:
                    lenght = (value as byte[])?.Length ?? 0;
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

        private static int GetMaxLenght(QRCodeInputType inputType)
        {
            return inputType switch
            {
                QRCodeInputType.Numeric => 7089,
                QRCodeInputType.Binary => 2953,
                QRCodeInputType.Kanji => 1817,
                _ => 4296,
            };
        }

        private static QRCodeInputType? GetInputType(object? value)
        {
            if (value == null) return null;

            return value switch
            {
                string => QRCodeInputType.Alphanumeric,
                sbyte or byte or short or ushort or int or uint or long or ulong or nint or nuint or double or float or decimal => QRCodeInputType.Numeric,
                byte[] => QRCodeInputType.Binary,
                _ => null,
            };
        }
    }
}