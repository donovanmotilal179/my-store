using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Distributed;
using MyStore.Implementation;
using MyStore.Interfaces;
using RedisCaching.CachingServices;
using StackExchange.Redis;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Net;

namespace MyStore.Pages.Animals
{
    public class IndexModel : PageModel
    {        
        public List<MyStore.Models.AnimalInfo> _listAnimals = new List<MyStore.Models.AnimalInfo>();
        public IGetAllAnimalSqlData _getAllAnimalSqlData;
        ICacheService _animalCacheService;
        public string animalErrorMessage;
        public string successMessage;

        public IndexModel(IGetAllAnimalSqlData getAllAnimalSqlData, ICacheService animalCacheService)
        {            
            _animalCacheService = animalCacheService;
            _getAllAnimalSqlData = getAllAnimalSqlData;
        }

        public void OnGet()
        {
            _listAnimals = _getAllAnimalSqlData.GetData(_animalCacheService);
        }
            
    }    
}
