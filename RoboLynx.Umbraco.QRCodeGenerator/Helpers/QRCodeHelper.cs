using RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Models;

namespace RoboLynx.Umbraco.QRCodeGenerator.Helpers
{
    public static class QRCodeHelper
    {
        public static IEnumerable<IQRCodeFormat> GetQRCodeFormatProviders()
        {
            var qrCodeTypes = GetTypes(typeof(IQRCodeFormat));

            return qrCodeTypes.Select(ct => Activator.CreateInstance(ct)).Cast<IQRCodeFormat>();
        }

        public static IEnumerable<IQRCodeType> GetQRCodeTypeProviders()
        {
            var qrCodeTypes = GetTypes(typeof(IQRCodeType));

            return qrCodeTypes.Select(ct => Activator.CreateInstance(ct)).Cast<IQRCodeType>();
        }

        public static IEnumerable<IQRCodeSource> GetQRCodeSourceProviders()
        {
            var qrCodeSource = GetTypes(typeof(IQRCodeSource));

            return qrCodeSource.Select(ct => Activator.CreateInstance(ct)).Cast<IQRCodeSource>();
        }

        public static IQRCodeFormat GetQRCodeFormatProviderById(string id, IQRCodeFormat defaultFormat)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException($"'{nameof(id)}' cannot be null or empty", nameof(id));
            }

            var qrCodeFormat = GetQRCodeFormatProviders().SingleOrDefault(s => s.Id.Equals(id, StringComparison.InvariantCultureIgnoreCase));
                
            return qrCodeFormat ?? defaultFormat;
        }
        public static IQRCodeType GetQRCodeTypeProviderByName(string name, IQRCodeSource source)
        {
            var qrCodeTyoe = GetTypes(typeof(IQRCodeType)).Single(s => s.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));

            return (IQRCodeType)Activator.CreateInstance(qrCodeTyoe, source);
        }

        public static IQRCodeSource GetQRCodeSourceProviderByName(string name, IPublishedContent publishedContent, string sourceSettings)
        {
            var qrCodeSource = GetTypes(typeof(IQRCodeSource)).Single(s=>s.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));

            return (IQRCodeSource)Activator.CreateInstance(qrCodeSource, publishedContent, sourceSettings);
        }

        public static bool StringToBoolean(string str, bool bDefault)
        {
            string[] BooleanStringOff = { "0" };

            if (string.IsNullOrEmpty(str))
                return bDefault;
            else if (BooleanStringOff.Contains(str, StringComparer.InvariantCultureIgnoreCase))
                return false;

            bool result;
            if (!bool.TryParse(str, out result))
                result = true;

            return result;
        }

        private static IEnumerable<Type> GetTypes(Type interfaceType)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => interfaceType.IsAssignableFrom(p) && !p.IsInterface && !p.IsAbstract);
        }


    }
}
