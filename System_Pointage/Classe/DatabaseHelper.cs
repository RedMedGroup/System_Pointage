using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System_Pointage.Classe
{
    public static class DatabaseHelper
    {
        public static List<string> GetDatabaseNames(string connectionString)
        {
            var databaseNames = new List<string>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("SELECT name FROM sys.databases", connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                databaseNames.Add(reader["name"].ToString());
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Échec de la récupération des noms des bases de données : {ex.Message}");
                }
            }

            return databaseNames;
        }
    }
}
