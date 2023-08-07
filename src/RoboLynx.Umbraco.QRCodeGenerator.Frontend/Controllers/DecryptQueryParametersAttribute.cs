using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Security.Cryptography;

namespace RoboLynx.Umbraco.QRCodeGenerator.Frontend.Controllers
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class DecryptQueryParametersAttribute : Attribute, IResourceFilter
    {
        private readonly IQueryCipher _queryCipher;
        private readonly ILogger<DecryptQueryParametersAttribute> _logger;

        public DecryptQueryParametersAttribute(IQueryCipher queryCipher, ILogger<DecryptQueryParametersAttribute> logger)
        {
            _queryCipher = queryCipher;
            _logger = logger;
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {

        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            if (context.HttpContext.Request.Query.ContainsKey(Frontend.CryptoParamName) && context.HttpContext.Request.Query[Frontend.CryptoParamName].ToString() != null)
            {
                try
                { 
                    string decrptedString = _queryCipher.DecryptQuery(context.HttpContext.Request.Query[Frontend.CryptoParamName].ToString());

                    var query = QueryHelpers.ParseQuery(decrptedString);

                    if (query == null || !query.Any())
                    {
                        _logger.LogWarning("Parsing of request query failed.");
                        context.Result = new BadRequestResult();
                        return;
                    }

                    context.HttpContext.Request.QueryString = QueryString.Create(query);
                }
                catch (CryptographicException)
                {
                    _logger.LogWarning("Decryption of request query failed.");
                    context.Result = new BadRequestResult();
                }
            }
            else {
                if (_queryCipher.OnlyCriptedCalls)
                {
                    context.Result = new NotFoundResult();
                };
            }
        }
    }
}
