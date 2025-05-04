using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Server.IIS.Core;
using MyStore.Implementation;
using MyStore.Interfaces;
using MyStore.Models;
using RedisCaching.CachingServices;
using StackExchange.Redis;
using System.Data.SqlClient;
using System.Net;
using System.Data;

namespace MyStore.Pages.Animals
{ 
    public class EditModel : PageModel
    {
        public string? animalErrorMessage;
        public string? animalSuccessMessage;
        private readonly IGetAllAnimalSqlData _getAllAnimalSqlData;
        private readonly IUpdateAnimalData _updateAnimalData;
        ICacheService _animalCacheService;
        public MyStore.Models.AnimalInfo animalInfo = new MyStore.Models.AnimalInfo();

        public EditModel(IGetAllAnimalSqlData getAllAnimalSqlData, ICacheService animalCacheService, IUpdateAnimalData updateAnimalData)
        {
            _getAllAnimalSqlData = getAllAnimalSqlData;
            _animalCacheService = animalCacheService;
            _updateAnimalData = updateAnimalData;
            
        }
        public void OnGet()
        {
            string? id = Request.Query["id"];
            var animal = _getAllAnimalSqlData.GetCacheDataById(_animalCacheService, id);
            string secondId = id;

            animalInfo.name = animal.name;
            animalInfo.type = animal.type;
            animalInfo.diet = animal.diet;
            animalInfo.supplements = animal.supplements;
        }

        public void OnPost()
        {
            animalInfo.id = Request.Query["id"].ToString();
            animalInfo.name = Request.Form["name"].ToString();
            animalInfo.type = Request.Form["type"].ToString();
            animalInfo.diet = Request.Form["diet"].ToString();
            animalInfo.supplements = Request.Form["supplements"].ToString();

            if (animalInfo != null)
            {
                if (animalInfo.name.Length == 0 || animalInfo.type.Length == 0 || animalInfo.diet.Length == 0 ||
                    animalInfo.supplements.Length == 0)
                {
                    animalErrorMessage = "All the fields are required.";
                    return;
                }
                else
                {                    
                    try
                    {
                        animalSuccessMessage = _updateAnimalData.UpdateDB(animalInfo.id, _animalCacheService, animalInfo);                        
                    }
                    catch(Exception ex)
                    {
                        animalErrorMessage = ex.Message;                  }                   
                }
            }
            else
            {
                animalErrorMessage = "All the fields are required.";
                return;
            }           
        }
    }
}
