using System;
using System.ComponentModel.DataAnnotations;

namespace RoboLynx.Umbraco.QRCodeGenerator.Cache
{
    public class QRCodeCacheOptions
    {
        /// <summary>
        /// Disable this cache
        /// </summary>
        public bool Disable { get; set; }

        /// <summary>
        /// Number of days cache storing
        /// </summary>
        [Range(0, 365, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public double MaxDays { get; set; } = 365;

        /// <summary>
        /// Delay before first running hosted service clearing cache. Hosted service is running only on master server.
        /// </summary>
        public TimeSpan DelayCleanCache { get; set; } = TimeSpan.FromMinutes(1);

        /// <summary>
        /// Running period hosted service clearing cache. Hosted service is running only on master server.
        /// </summary>
        public TimeSpan PeriodCleanCache { get; set; } = TimeSpan.FromHours(3);
    }
}