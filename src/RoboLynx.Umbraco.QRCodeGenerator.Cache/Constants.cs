using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboLynx.Umbraco.QRCodeGenerator.Cache
{
    public static class Constants
    {
        public static string DefaultLocalCacheLocation => $"~/App_Data/TEMP/QRCodeGeneratorCache/{BackofficeCacheName}";
        public const string BackofficeCacheName = "Backoffice";

        public static class Configuration
        {
            public const string DisableKey = "QRCodeGenerator.Disable";
            public const string LocationKey = "QRCodeGenerator.Location";
            public const string MaxDaysKey = "QRCodeGenerator.MaxDays";
        }
    }
}
