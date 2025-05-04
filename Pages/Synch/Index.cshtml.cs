using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyStore.Interfaces;
using RedisCaching.CachingServices;

namespace MyStore.Pages.Synch
{
    public class IndexModel : PageModel
    {
        public List<MyStore.Models.ClientInfo> _listClients = new List<MyStore.Models.ClientInfo>();
        public IGetAllClientSqlData _getAllSqlData;
        ICacheService _cacheService;

        public IndexModel(IGetAllClientSqlData getAllSqlData, ICacheService cacheService)
        {
            _cacheService = cacheService;
            _getAllSqlData = getAllSqlData;
        }

        public void OnGet()
        {
            //synch cache and database and redirect to clients index page with a popup message to confirm synch success or failure.
            _listClients = _getAllSqlData.GetData(_cacheService);

        }
      
    }
}
