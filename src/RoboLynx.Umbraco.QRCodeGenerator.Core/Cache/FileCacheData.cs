using System;

namespace RoboLynx.Umbraco.QRCodeGenerator.Cache
{
    public class FileCacheData
    {
        public FileCacheData(string hashId, string path, DateTimeOffset expiryDate, DateTimeOffset lastModifiedDate)
        {
            HashId = hashId;
            Path = path;
            ExpiryDate = expiryDate;
            LastModifiedDate = lastModifiedDate;
        }

        public string HashId { get; set; }

        public string Path { get; set; }

        public DateTimeOffset ExpiryDate { get; set; }

        public DateTimeOffset LastModifiedDate { get; set; }
    }
}