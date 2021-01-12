using RoboLynx.Umbraco.QRCodeGenerator.Exceptions;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;
using Umbraco.Core.Services;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes
{
    public abstract class QRCodeType : IQRCodeType
    {
        protected readonly IQRCodeSource source;
        protected readonly IDictionary<string, IEnumerable<IQRCodeTypeValidator>> validators;

        public QRCodeType(IQRCodeSource source)
        {
            this.source = source;
            validators = new Dictionary<string, IEnumerable<IQRCodeTypeValidator>>();
        }

        public string Name => ApplicationContext.Current.Services.TextService.Localize($"qrCodeTypes/{GetType().Name.ToFirstLower()}Name") ?? GetType().Name;

        public string Description => ApplicationContext.Current.Services.TextService.Localize($"qrCodeTypes/{GetType().Name.ToFirstLower()}Description") ?? string.Empty;

        public abstract string Value { get; }

        protected void RunValidator(string key, object value)
        {
            if (validators.ContainsKey(key) && validators[key].Any())
            {
                foreach (var validator in validators[key])
                {
                    if (!validator.Validate(value, out string message))
                    {
                        throw new ValidationQRCodeGeneratorException(GetType(), key, message);
                    }
                }
            }
        }

        protected void CheckSource()
        {
            if (source == null)
            {
                throw new System.ArgumentNullException(nameof(source), "Class is not configure to use.");
            }
        }
    }
}
