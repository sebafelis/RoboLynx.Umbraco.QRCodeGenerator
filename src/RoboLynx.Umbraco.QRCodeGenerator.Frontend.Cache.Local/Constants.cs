using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrontendConstants = RoboLynx.Umbraco.QRCodeGenerator.Frontend.Constants;
namespace RoboLynx.Umbraco.QRCodeGenerator.Frontend.Cache
{
    internal static class Constants
    {
        public static string DefaultFrontendCacheLocation => $"~/App_Data/TEMP/QRCodeGeneratorCache/{FrontendConstants.FrontendCacheName}";
    }
}
