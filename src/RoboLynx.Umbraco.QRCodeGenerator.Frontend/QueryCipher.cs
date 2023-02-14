using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboLynx.Umbraco.QRCodeGenerator.Frontend
{
    public class QueryCipher : IQueryCipher
    {
        IDataProtector _protector;
        private readonly IOptions<QRCodeFrontendOptions> _options;

        public QueryCipher(IDataProtectionProvider dataProtectionProvider, IOptions<QRCodeFrontendOptions> options)
        {
            _protector = dataProtectionProvider.CreateProtector("QRCodeGenerator.QueryStrings");
            _options = options;
        }
        public bool OnlyCriptedCalls => _options.Value.OnlyEncryptedCalls;

        public string EncryptQuery(string value)
        {
            return _protector.Protect(value);
        }

        public string DecryptQuery(string value)
        {
            return _protector.Unprotect(value);
        }
    }
}
