using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyStore.Models;
using System.Data.SqlClient;

namespace MyStore.Pages.Clients
{
    public class CreateModel : PageModel
    {
        public string clientErrorMessage;
        public string clientSuccessMessage;
        public string clientFailureMessage;

        public ClientInfo clientInfo = new ClientInfo();
        public void OnGet()
        {

        }

        public void OnPost()
        {
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
                    //save the client to the database
                    try
                    {
                        String connectionstring = "Data Source=DONPC\\MSSQLSERVER01;Initial Catalog=mystore;Integrated Security=True;Connect Timeout=30;Encrypt=False;";

                        using (SqlConnection connection = new SqlConnection(connectionstring))
                        {
                            connection.Open();
                            String sql = "INSERT INTO clients (name, email, phone, address) values " +
                                "(@name, @email, @phone, @address)";

                            using (SqlCommand command = new SqlCommand(sql, connection))
                            {
                                command.Parameters.AddWithValue("@name", clientInfo.name);
                                command.Parameters.AddWithValue("@email", clientInfo.email);
                                command.Parameters.AddWithValue("@phone", clientInfo.phone);
                                command.Parameters.AddWithValue("@address", clientInfo.address);
                                command.ExecuteNonQuery();
                            }
                        }
                        clientSuccessMessage = "New Client have been captured.";
                        clientInfo.name = "";
                        clientInfo.phone = "";
                        clientInfo.email = "";
                        clientInfo.address = "";
                    }
                    catch (Exception ex)
                    {
                        clientErrorMessage = ex.Message;
                    }
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
