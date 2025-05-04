using Microsoft.Extensions.Diagnostics.HealthChecks;
using MyStore.Interfaces;
using RedisCaching.CachingServices;
using System.Data.SqlClient;

namespace MyStore.Implementation
{
    public class UpdateAnimalData : IUpdateAnimalData
    {
        public List<MyStore.Models.AnimalInfo>? _listAnimals;
        public MyStore.Models.AnimalInfo? _animalInfo;
        private object _lock = new object();
        public string? successMessage;
        public string? clientErrorMessage;

        public string UpdateDB(string id, ICacheService _cacheService, MyStore.Models.AnimalInfo? animalInfo)
        {
            _animalInfo = new MyStore.Models.AnimalInfo();
            
            try
            {
                //Update the database

                String connectionstring = "Data Source=DONPC\\MSSQLSERVER01;Initial Catalog=mystore;Integrated Security=True;Connect Timeout=30;Encrypt=False;";

                using (SqlConnection connection = new SqlConnection(connectionstring))
                {
                    connection.Open();
                    String sql = "UPDATE animals set " +
                        "name = @name, " +
                        "type = @type, " +
                        "diet = @diet, " +
                        "supplements = @supplements " +
                        "WHERE id = @id ";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", animalInfo.name);
                        command.Parameters.AddWithValue("@type", animalInfo.type);
                        command.Parameters.AddWithValue("@diet", animalInfo.diet);
                        command.Parameters.AddWithValue("@supplements", animalInfo.supplements);
                        command.Parameters.AddWithValue("@id", animalInfo.id);                       
                        command.ExecuteNonQuery();
                    }
                }
                successMessage = "Animal details have been updated.";

                animalInfo.name = "";
                animalInfo.type = "";
                animalInfo.diet = "";
                animalInfo.supplements = "";

                return successMessage;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}