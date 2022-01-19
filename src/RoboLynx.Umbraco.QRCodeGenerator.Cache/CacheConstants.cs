using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboLynx.Umbraco.QRCodeGenerator.Constants
{
    public static class Cache
    {
        public static string DefaultLocalCacheLocation => $"QRCodeGeneratorCache\\{Backoffice.BackofficeCacheName}";
    }
}
