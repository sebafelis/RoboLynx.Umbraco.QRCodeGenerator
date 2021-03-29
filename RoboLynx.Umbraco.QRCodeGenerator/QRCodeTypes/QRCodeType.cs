using RoboLynx.Umbraco.QRCodeGenerator.Exceptions;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes.Validators;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.Services;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes
{
    public abstract class QRCodeType : IQRCodeType
    {
        protected readonly ILocalizedTextService localizedTextService;
        protected readonly IDictionary<string, IEnumerable<IQRCodeTypeValidator>> validators;

        public QRCodeType(ILocalizedTextService localizedTextService)
        {
            this.localizedTextService = localizedTextService;
            validators = new Dictionary<string, IEnumerable<IQRCodeTypeValidator>>();
        }

        public abstract string Id { get; }

        public virtual string Name => localizedTextService.Localize($"qrCodeTypes/{GetType().Name.ToFirstLower()}Name") ?? GetType().Name;

        public virtual string Description => localizedTextService.Localize($"qrCodeTypes/{GetType().Name.ToFirstLower()}Description") ?? string.Empty;

        public abstract string Value(IQRCodeSource source, string sourceSettings, IPublishedContent content, string culture);

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
    }
}
