using System;

namespace RoboLynx.Umbraco.QRCodeGenerator.Cache
{
    public class FileCacheData
    {
        public string HashId { get; set; }

        public string Path { get; set; }

        public DateTimeOffset ExpiryDate { get; set; }

        public DateTimeOffset LastModifiedDate { get; set; }
    }
}