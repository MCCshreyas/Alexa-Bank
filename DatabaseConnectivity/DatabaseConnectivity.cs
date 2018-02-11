using System;
using System.IO;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BankApplicationLibrary
{
    public class DatabaseConnectivity
    {
        private string Server { get; set; }
        private string UserName { get; set; }
        private string Password { get; set; }
        public string DatabaseName { get; private set; }

        public DatabaseConnectivity()
        {
            ReadDatabaseInfoFromJson();
        }

        private void ReadDatabaseInfoFromJson()
        {
            try
            {
                var data = File.ReadAllText("Database.txt");
                var obj = JsonConvert.DeserializeObject<JObject>(data);
                this.Server = (string)obj["Server"];
                this.UserName = (string) obj["Username"];
                this.Password = (string)obj["Password"];
                this.DatabaseName = (string)obj["DatabaseName"];
            }
            catch (JsonException e)
            {
                Console.WriteLine(e);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public MySqlConnection Connect()
        {
            var connectionString = $"Server={Server}; " +
                                   $"database={DatabaseName}; " +
                                   $"UID={UserName}; " +
                                   $"password={Password}";
            var con = new MySqlConnection(connectionString);
            return con;
        }
    }
}
