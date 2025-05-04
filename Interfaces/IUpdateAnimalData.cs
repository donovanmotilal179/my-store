using RedisCaching.CachingServices;

namespace MyStore.Interfaces
{
    public interface IUpdateAnimalData
    {
        public string UpdateDB(string id, ICacheService _cacheService, MyStore.Models.AnimalInfo? animalInfo);
    }
}
