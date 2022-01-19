using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboLynx.Umbraco.QRCodeGenerator.Constants
{
    public static class Frontend
    {                            
        public const string FrontendApiOptionSectionName = "FrontendApi";

        private const string _frontendCacheNameValue = "Frontend";

        public static string FrontendCacheName { get; set; } = _frontendCacheNameValue;                 
    }                         
}
