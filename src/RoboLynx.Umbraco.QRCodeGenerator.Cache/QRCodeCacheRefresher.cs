using Newtonsoft.Json;
using System;
using Umbraco.Core.Cache;

namespace RoboLynx.Umbraco.QRCodeGenerator.Cache
{
    public enum QRCodeCacheChangeType { Add, Remove }

    public abstract class QRCodeCacheRefresher<T> : PayloadCacheRefresherBase<QRCodeCacheRefresher<T>, QRCodeCacheRefresher<T>.JsonPayload>
    {
        public TimeSpan? Timeout { get; }

        public QRCodeCacheRefresher(AppCaches appCaches, TimeSpan? timeout) : base(appCaches)
        {
            Timeout = timeout;
        }

        public override void Refresh(JsonPayload[] payloads)
        {
            var isolatedCache = AppCaches.IsolatedCaches.GetOrCreate<T>();

            foreach (var payload in payloads)
            {
                if (payload.ChangeType == QRCodeCacheChangeType.Remove)
                {
                    isolatedCache.Clear(payload.HashId);
                }

                if (payload.ChangeType == QRCodeCacheChangeType.Add)
                {
                    isolatedCache.InsertCacheItem(payload.HashId, () => new FileCacheData
                    {
                        HashId = payload.HashId,
                        Path = payload.Path,
                        ExpiryDate = payload.ExpiryDate.Value
                    }, Timeout);
                }
            }

            base.Refresh(payloads);
        }

        

        public override void RefreshAll()
        {
            var isolatedCache = AppCaches.IsolatedCaches.GetOrCreate<T>();

            isolatedCache.Clear();
        }

        // these events should never trigger
        // everything should be PAYLOAD/JSON

        public override void Refresh(int id) => throw new NotSupportedException();

        public override void Refresh(Guid id) => throw new NotSupportedException();

        public override void Remove(int id) => throw new NotSupportedException();

        public class JsonPayload
        {
            [JsonConstructor]
            public JsonPayload(string hashId, QRCodeCacheChangeType changeType, string path = null, DateTimeOffset? expiryDate = null)
            {
                HashId = hashId;
                ExpiryDate = expiryDate;
                Path = path;
                ChangeType = changeType;
            }

            public string HashId { get; }
            public string Path { get; }
            public DateTimeOffset? ExpiryDate { get; }
            public QRCodeCacheChangeType ChangeType { get; }
        }


    }
}
