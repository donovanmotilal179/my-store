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

namespace MyStore.Pages.Clients
{
    public class IndexModel : PageModel
    {        
        public List<MyStore.Models.ClientInfo> _listClients = new List<MyStore.Models.ClientInfo>();
        public IGetAllClientSqlData _getAllSqlData;
        ICacheService _cacheService;
        public string clientErrorMessage;
        public string successMessage;

        public IndexModel(IGetAllClientSqlData getAllSqlData, ICacheService cacheService)
        {            
            _cacheService = cacheService;
            _getAllSqlData = getAllSqlData;
        }

        public void OnGet()
        {
            _listClients = _getAllSqlData.GetData(_cacheService);
        }
            
    }    
}
