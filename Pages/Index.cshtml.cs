using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyStore.Implementation;
using MyStore.Interfaces;
using RedisCaching.CachingServices;

namespace MyStore.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        

        public IndexModel(ILogger<IndexModel> logger)
        {
        }

        public void OnGet()
        {            
            
        }
    }
}