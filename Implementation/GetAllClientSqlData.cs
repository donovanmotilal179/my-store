using System.Data.SqlClient;
using MyStore.Interfaces;
using MyStore.Pages.Clients;
using StackExchange.Redis;
using static MyStore.Pages.Clients.IndexModel;
using RedisCaching.CachingServices;
using MyStore.Models;
using System.Collections;

namespace MyStore.Implementation
{
    public class GetAllClientSqlData : IGetAllClientSqlData
    {
        public List<MyStore.Models.ClientInfo>? _listClients;
        public MyStore.Models.ClientInfo? _client;
        private object _lock = new object();

        public List<MyStore.Models.ClientInfo> GetData(ICacheService _cacheService)
        {
            _listClients = new List<MyStore.Models.ClientInfo>();
            _client = new MyStore.Models.ClientInfo();

            try
            {

                String connectionstring = "Data Source=DONPC\\MSSQLSERVER01;Initial Catalog=mystore;Integrated Security=True;Connect Timeout=30;Encrypt=False;";

                using (SqlConnection connection = new SqlConnection(connectionstring))
                {
                    connection.Open();
                    String sql = "SELECT * FROM clients";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                MyStore.Models.ClientInfo clientInfo = new MyStore.Models.ClientInfo();
                                clientInfo.id = "" + reader.GetInt32(0);
                                clientInfo.name = reader.GetString(1);
                                clientInfo.email = reader.GetString(2);
                                clientInfo.phone = reader.GetString(3);
                                clientInfo.address = reader.GetString(4);
                                clientInfo.createdAt = reader.GetDateTime(5).ToString();

                                _listClients.Add(clientInfo);
                            }

                        }
                    }
                }
                _cacheService.RemoveData("clientInfo");
                var cacheData = _cacheService.GetData<IEnumerable<MyStore.Models.ClientInfo>>("clientInfo");

                if (cacheData != null)
                {
                    return cacheData.ToList();
                }
                lock (_lock)
                {
                    var expirationTime = DateTimeOffset.Now.AddMinutes(5.0);
                    cacheData = _listClients;
                    _cacheService.SetData<IEnumerable<MyStore.Models.ClientInfo>>("clientInfo", cacheData, expirationTime);
                }
                return cacheData.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.ToString());
            }
            return _listClients;
        }

         

        //public MyStore.Models.ClientInfo GetDataById(ICacheService _cacheService, string id)
        //{
        //    _listClients = new List<MyStore.Models.ClientInfo>();
        //    _client = new MyStore.Models.ClientInfo();

        //    try
        //    {
        //        String connectionstring = "Data Source=DONPC\\MSSQLSERVER01;Initial Catalog=mystore;Integrated Security=True;Connect Timeout=30;Encrypt=False;";

        //        using (SqlConnection connection = new SqlConnection(connectionstring))
        //        {
        //            connection.Open();
        //            String sql = "SELECT * FROM clients";

        //            using (SqlCommand command = new SqlCommand(sql, connection))
        //            {
                                                
        //                using (SqlDataReader reader = command.ExecuteReader())
        //                {
        //                    while (reader.Read())
        //                    {
        //                        MyStore.Models.ClientInfo clientInfo = new MyStore.Models.ClientInfo();

        //                        _client.id = "" + reader.GetInt32(0);
        //                        _client.name = reader.GetString(1);
        //                        _client.email = reader.GetString(2);
        //                        _client.phone = reader.GetString(3);
        //                        _client.address = reader.GetString(4);

        //                        _listClients.Add(clientInfo);
        //                    }

        //                }
        //            }
        //        }

        //        var cacheData = _cacheService.GetData<IEnumerable<MyStore.Models.ClientInfo>>("clientInfo");

        //        if (cacheData != null)
        //        {
        //            return cacheData.ToList().Where(x => x.id == id).First();
        //        }
        //        lock (_lock)
        //        {
        //            var expirationTime = DateTimeOffset.Now.AddMinutes(5.0);
        //            cacheData = _listClients;
        //            _cacheService.SetData<IEnumerable<MyStore.Models.ClientInfo>>("clientInfo", cacheData, expirationTime);
        //        }
        //        return cacheData.ToList().Where(x => x.id == id).First();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Error : " + ex.ToString());
        //        return null;
        //    }                        
        //}


        public MyStore.Models.ClientInfo GetCacheDataById(ICacheService _cacheService, string id)
        {
            _listClients = new List<MyStore.Models.ClientInfo>();
            _client = new MyStore.Models.ClientInfo();

            try
            {                

                var cacheData = _cacheService.GetData<IEnumerable<MyStore.Models.ClientInfo>>("clientInfo");
                return cacheData.ToList().Where(x => x.id == id).First();                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.ToString());
                throw new Exception (ex.Message);
            }
        }

        public List<MyStore.Models.ClientInfo> GetCacheData(ICacheService _cacheService)
        {
            _listClients = new List<MyStore.Models.ClientInfo>();
            _client = new MyStore.Models.ClientInfo();

            try
            {
                var cacheData = _cacheService.GetData<IEnumerable<MyStore.Models.ClientInfo>>("clientInfo");

                return cacheData.ToList();                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.ToString());
                throw new Exception(ex.Message);
            }
        }
    }    
}
