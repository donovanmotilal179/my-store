using System.Data.SqlClient;
using MyStore.Interfaces;
using MyStore.Pages.Animals;
using StackExchange.Redis;
using static MyStore.Pages.Animals.IndexModel;
using RedisCaching.CachingServices;
using MyStore.Models;
using System.Collections;

namespace MyStore.Implementation
{
    public class GetAllAnimalSqlData : IGetAllAnimalSqlData
    {
        public List<MyStore.Models.AnimalInfo>? _listAnimals;
        public MyStore.Models.AnimalInfo? _animal;
        private object _lock = new object();

        public List<MyStore.Models.AnimalInfo> GetData(ICacheService _animalCacheService)
        {
            _listAnimals = new List<MyStore.Models.AnimalInfo>();
            _animal = new MyStore.Models.AnimalInfo();

            try
            {

                String connectionstring = "Data Source=DONPC\\MSSQLSERVER01;Initial Catalog=mystore;Integrated Security=True;Connect Timeout=30;Encrypt=False;";

                using (SqlConnection connection = new SqlConnection(connectionstring))
                {
                    connection.Open();
                    String sql = "SELECT * FROM animals";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                MyStore.Models.AnimalInfo animalInfo = new MyStore.Models.AnimalInfo();
                                animalInfo.id = "" + reader.GetInt32(0);
                                animalInfo.name = reader.GetString(1);
                                animalInfo.type = reader.GetString(2);
                                animalInfo.diet = reader.GetString(3);
                                animalInfo.supplements = reader.GetString(4);
                                animalInfo.createdAt = reader.GetDateTime(5).ToString();

                                _listAnimals.Add(animalInfo);
                            }

                        }
                    }
                }
                _animalCacheService.RemoveData("animalInfo");
                var cacheData = _animalCacheService.GetData<IEnumerable<MyStore.Models.AnimalInfo>>("animalInfo");

                if (cacheData != null)
                {
                    return cacheData.ToList();
                }
                lock (_lock)
                {
                    var expirationTime = DateTimeOffset.Now.AddMinutes(5.0);
                    cacheData = _listAnimals;
                    _animalCacheService.SetData<IEnumerable<MyStore.Models.AnimalInfo>>("animalInfo", cacheData, expirationTime);
                }
                return cacheData.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.ToString());
            }
            return _listAnimals;
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


        public MyStore.Models.AnimalInfo GetCacheDataById(ICacheService _animalCacheService, string id)
        {
            _listAnimals = new List<MyStore.Models.AnimalInfo>();
            _animal = new MyStore.Models.AnimalInfo();

            try
            {                

                var cacheData = _animalCacheService.GetData<IEnumerable<MyStore.Models.AnimalInfo>>("animalInfo");
                return cacheData.ToList().Where(x => x.id == id).First();                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.ToString());
                throw new Exception (ex.Message);
            }
        }

        public List<MyStore.Models.AnimalInfo> GetCacheData(ICacheService _animalCacheService)
        {
            _listAnimals = new List<MyStore.Models.AnimalInfo>();
            _animal = new MyStore.Models.AnimalInfo();

            try
            {
                var cacheData = _animalCacheService.GetData<IEnumerable<MyStore.Models.AnimalInfo>>("animalInfo");

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
