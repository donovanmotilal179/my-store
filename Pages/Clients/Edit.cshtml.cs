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

namespace MyStore.Pages.Clients
{
    public class EditModel : PageModel
    {
        public string? clientErrorMessage;
        public string? clientSuccessMessage;
        private readonly IGetAllClientSqlData _getAllClientSqlData;
        private readonly IUpdateClientData _updateClientData;
        ICacheService _clientCacheService;
        public MyStore.Models.ClientInfo clientInfo = new MyStore.Models.ClientInfo();

        public EditModel(IGetAllClientSqlData getAllClientSqlData, ICacheService clientCacheService, IUpdateClientData updateClientData)
        {
            _getAllClientSqlData = getAllClientSqlData;
            _clientCacheService = clientCacheService;
            _updateClientData = updateClientData;
            
        }
        public void OnGet()
        {
            string? id = Request.Query["id"];
            var client = _getAllClientSqlData.GetCacheDataById(_clientCacheService, id);

            clientInfo.name = client.name;
            clientInfo.phone = client.phone;
            clientInfo.email = client.email;
            clientInfo.address = client.address;
        }

        public void OnPost()
        {
            clientInfo.id = Request.Query["id"].ToString();
            clientInfo.name = Request.Form["name"].ToString();
            clientInfo.phone = Request.Form["phone"].ToString();
            clientInfo.email = Request.Form["email"].ToString();
            clientInfo.address = Request.Form["address"].ToString();

            if (clientInfo != null)
            {
                if (clientInfo.name.Length == 0 || clientInfo.phone.Length == 0 || clientInfo.email.Length == 0 ||
                    clientInfo.address.Length == 0)
                {
                    clientErrorMessage = "All the fields are required.";
                    return;
                }
                else
                {                    
                    try
                    {
                        clientSuccessMessage = _updateClientData.UpdateDB(clientInfo.id, _clientCacheService, clientInfo);
                    }
                    catch(Exception ex)
                    {
                        clientErrorMessage = ex.Message;                  }                   
                }
            }
            else
            {
                clientErrorMessage = "All the fields are required.";
                return;
            }           
        }
    }
}
