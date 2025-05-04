using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyStore.Models;
using System.Data.SqlClient;

namespace MyStore.Pages.Animals
{
    public class CreateModel : PageModel
    {
        public string animalErrorMessage;
        public string animalSuccessMessage;
        public AnimalInfo animalInfo = new AnimalInfo();
        public void OnGet()
        {

        }

        public void OnPost()
        {
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
                    //save the client to the database
                    try
                    {
                        String connectionstring = "Data Source=DONPC\\MSSQLSERVER01;Initial Catalog=mystore;Integrated Security=True;Connect Timeout=30;Encrypt=False;";

                        using (SqlConnection connection = new SqlConnection(connectionstring))
                        {
                            connection.Open();
                            String sql = "INSERT INTO animals (name, type, diet, supplements) values " +
                                "(@name, @type, @diet, @supplements)";

                            using (SqlCommand command = new SqlCommand(sql, connection))
                            {
                                command.Parameters.AddWithValue("@name", animalInfo.name);
                                command.Parameters.AddWithValue("@type", animalInfo.type);
                                command.Parameters.AddWithValue("@diet", animalInfo.diet);
                                command.Parameters.AddWithValue("@supplements", animalInfo.supplements);
                                command.ExecuteNonQuery();
                            }
                        }
                        animalSuccessMessage = "New Animal have been captured.";
                        animalInfo.name = "";
                        animalInfo.type = "";
                        animalInfo.diet = "";
                        animalInfo.supplements = "";
                    }
                    catch (Exception ex)
                    {
                        animalErrorMessage = ex.Message;
                    }
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
