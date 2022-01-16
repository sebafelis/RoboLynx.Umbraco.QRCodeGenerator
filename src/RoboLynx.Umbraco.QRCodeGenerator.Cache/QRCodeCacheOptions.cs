using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboLynx.Umbraco.QRCodeGenerator.Cache
{
    public class QRCodeCacheOptions
    {
        /// <summary>
        /// Disable this cache
        /// </summary>
        public bool Disable { get; set; }

        /// <summary>
        /// When <c>true</c> redirect request to public URL where cache file is store.
        /// </summary>
        public bool RedirectToCacheLocation { get; set; }

        /// <summary>
        /// Directory where cache is stored on server
        /// </summary>
        public string CacheLocation { get; set; }

        /// <summary>
        /// Number of days cache storing
        /// </summary>
        public double MaxDays { get; set; }

        /// <summary>
        /// Delay before first running hosted service clearing cache. Hosted service is running only on master server.
        /// </summary>
        public TimeSpan DelayCleanCache { get; set; } = TimeSpan.FromMinutes(1);

        /// <summary>
        /// Running period hosted service clearing cache. Hosted service is running only on master server.
        /// </summary>
        public TimeSpan PeriodCleanCache { get; set; } = TimeSpan.FromHours(1);

    }
}
