using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboLynx.Umbraco.QRCodeGenerator.Cache
{
    public class FileCacheData
    {
        public string HashId { get; set; }

        public string Path { get; set; }

        public DateTimeOffset ExpiryDate { get; set; }
    }
}
