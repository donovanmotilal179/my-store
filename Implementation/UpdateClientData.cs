using Microsoft.Extensions.Diagnostics.HealthChecks;
using MyStore.Interfaces;
using RedisCaching.CachingServices;
using System.Data.SqlClient;

namespace MyStore.Implementation
{
    public class UpdateClientData : IUpdateClientData
    {
        public List<MyStore.Models.ClientInfo>? _listClients;
        public MyStore.Models.ClientInfo? _clientInfo;
        private object _lock = new object();
        public string? successMessage;
        public string? errorMessage;

        public string UpdateDB(string id, ICacheService _cacheService, MyStore.Models.ClientInfo? clientInfo)
        {
            _clientInfo = new MyStore.Models.ClientInfo();
            
            try
            {
                //Update the database

                String connectionstring = "Data Source=DONPC\\MSSQLSERVER01;Initial Catalog=mystore;Integrated Security=True;Connect Timeout=30;Encrypt=False;";

                using (SqlConnection connection = new SqlConnection(connectionstring))
                {
                    connection.Open();
                    String sql = "UPDATE clients set " +
                        "name = @name, " +
                        "email = @email, " +
                        "phone = @phone, " +
                        "address = @address " +
                        "WHERE id = @id ";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", clientInfo.name);
                        command.Parameters.AddWithValue("@email", clientInfo.email);
                        command.Parameters.AddWithValue("@phone", clientInfo.phone);
                        command.Parameters.AddWithValue("@address", clientInfo.address);
                        command.Parameters.AddWithValue("@id", clientInfo.id);                       
                        command.ExecuteNonQuery();
                    }
                }
                successMessage = "Client details have been updated.";                

                clientInfo.name = "";
                clientInfo.phone = "";
                clientInfo.email = "";
                clientInfo.address = "";

                return successMessage;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}