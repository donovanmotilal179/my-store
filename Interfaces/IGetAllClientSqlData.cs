using MyStore.Models;
using RedisCaching.CachingServices;

namespace MyStore.Interfaces
{
    public interface IGetAllClientSqlData
    {
        public List<ClientInfo> GetData(ICacheService _cacheService);
        public List<ClientInfo> GetCacheData(ICacheService _cacheService);
        //public MyStore.Models.ClientInfo GetDataById(ICacheService _cacheService, string id);

        public MyStore.Models.ClientInfo GetCacheDataById(ICacheService _cacheService, string id);
    }
}
