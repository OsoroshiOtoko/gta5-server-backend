using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;
using GTANetworkAPI;
using MySql.Data.MySqlClient;

namespace otokomod.MySQL
{
    class DB
    {
        public static MySqlConnection connection;
        private string _host { get; set; }
        private string _user { get; set; }
        private string _pass { get; set; }
        private string _base { get; set; }

        private DB()
        {
            _host = "localhost";
            _user = "root";
            _pass = "Putat2156";
            _base = "otokomod_server_gta5";
        }

        public static void InitConnection()
        {
            DB sql = new DB();
            string SQLconnection = $"SERVER={sql._host}; DATABASE={sql._base}; UID={sql._user}; PASSWORD={sql._pass}";
            connection = new MySqlConnection(SQLconnection);

            try
            {
                connection.Open();
                NAPI.Util.ConsoleOutput("Successful database connection: " + sql._base);
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput("Unsuccessful database connection: " + sql._base);
                NAPI.Util.ConsoleOutput("Exception: " + ex.ToString());
                NAPI.Task.Run(() =>
                {
                    Environment.Exit(0);
                }, delayTime: 5000);

            }
        }

        
    }
}
