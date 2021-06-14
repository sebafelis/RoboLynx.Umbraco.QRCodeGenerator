using RoboLynx.Umbraco.QRCodeGenerator.Exceptions;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes
{
    public abstract class QRCodeTypeValue : IQRCodeTypeValueFactory
    {
        protected IDictionary<string, IEnumerable<IQRCodeTypeValidator>> Validators { get; set; }

        public QRCodeTypeValue()
        {
            Validators = new Dictionary<string, IEnumerable<IQRCodeTypeValidator>>();
            Validators.Add("all", new List<IQRCodeTypeValidator>() { new LenghtValidation() });
        }

        /// <inheritdoc/>
        public abstract string Value(bool validate);

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
