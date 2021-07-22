using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboLynx.Umbraco.QRCodeGenerator.Cache
{
    public class QRCodeCacheConfig
    {
        public bool Disable { get; set; }
        public bool RedirectToCacheLocation { get; set; }
        public string CacheLocation { get; set; }
        public double MaxDays { get; set; }
    }
}
