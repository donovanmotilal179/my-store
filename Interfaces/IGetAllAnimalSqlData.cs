using MyStore.Models;
using RedisCaching.CachingServices;

namespace MyStore.Interfaces
{
    public interface IGetAllAnimalSqlData
    {
        public List<AnimalInfo> GetData(ICacheService _cacheService);
        public List<AnimalInfo> GetCacheData(ICacheService _cacheService);
        //public MyStore.Models.ClientInfo GetDataById(ICacheService _cacheService, string id);

        public MyStore.Models.AnimalInfo GetCacheDataById(ICacheService _cacheService, string id);
    }
}
