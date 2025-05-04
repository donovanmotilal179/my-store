using RedisCaching.CachingServices;

namespace MyStore.Interfaces
{
    public interface IUpdateClientData
    {
        public string UpdateDB(string id, ICacheService _cacheService, MyStore.Models.ClientInfo? clientInfo);
    }
}
