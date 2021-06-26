using RoboLynx.Umbraco.QRCodeGenerator.Exceptions;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes
{
    public abstract class QRCodeType : IQRCodeType
    {
        protected const string AllFieldsValidator = "all";

        protected IDictionary<string, IEnumerable<IQRCodeTypeValidator>> Validators { get; set; }

        public QRCodeType()
        {
            Validators = new Dictionary<string, IEnumerable<IQRCodeTypeValidator>>
            {
                { AllFieldsValidator, new List<IQRCodeTypeValidator>() { new LenghtValidation() } }
            };
        }

        /// <inheritdoc/>
        public abstract string GetCodeContent();

        protected void Validate(string key, object value)
        {
            if (Validators.ContainsKey(key) && Validators[key].Any())
            {
                foreach (var validator in Validators[key])
                {
                    if (!validator.Validate(value, out string message))
                    {
                        throw new ValidationQRCodeGeneratorException(GetType(), key, message);
                    }
                }
            }
        }
    }
}
