using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Diagnostics;
using System.Security.Principal;
using System.Text;
using GTANetworkAPI;
using GTANetworkMethods;
using MySql.Data.MySqlClient;

namespace otokomod.MySQL
{
    class DB
    {     

        public static bool Test(string database)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection($"SERVER=localhost; DATABASE=otokomod_{database}; UID=root; PASSWORD=Putat2156"))
                {
                    conn.Open();
                    NAPI.Util.ConsoleOutput($"[MySQL]Connection to otokomod_{database} is successful!");
                    conn.Close();
                }
                return true;
            }
            catch (ArgumentException ae)
            {
                NAPI.Util.ConsoleOutput($"Сonnection string contains an error\n{ae.ToString()}");
                return false;
            }
            catch (MySqlException me)
            {
                switch (me.Number)
                {
                    case 1042:
                        NAPI.Util.ConsoleOutput("Unable to connect to any of the specified MySQL hosts");
                        break;
                    case 0:
                        NAPI.Util.ConsoleOutput("Access denied");
                        break;
                    default:
                        NAPI.Util.ConsoleOutput($"({me.Number}) {me.Message}");
                        break;
                }
                return false;
            }
        }

        public static void Query(string database, MySqlCommand command)
        {
            try
            {
                if (command.CommandText.Length < 1) NAPI.Util.ConsoleOutput($"BAD Query?: '{command.CommandText}'");
                else
                {
                    using (MySqlConnection connection = new MySqlConnection($"SERVER=localhost; DATABASE=otokomod_{database}; UID=root; PASSWORD=Putat2156"))
                    {
                        connection.Open();

                        command.Connection = connection;

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                NAPI.Util.ConsoleOutput($"Query({command.CommandText}) Exception: {e.ToString()}");
            }
        }

        public static DataTable QueryRead(string database, MySqlCommand command)
        {
            try
            {
                if (command.CommandText.Length < 1)
                {
                    NAPI.Util.ConsoleOutput($"BAD QueryRead?: '{command.CommandText}'");
                    return null;
                }
                else
                {
                    using (MySqlConnection connection = new MySqlConnection($"SERVER=localhost; DATABASE=otokomod_{database}; UID=root; PASSWORD=Putat2156"))
                    {
                        connection.Open();

                        command.Connection = connection;

                        using (DbDataReader reader = command.ExecuteReader())
                        {
                            using (DataTable result = new DataTable())
                            {
                                result.Load(reader);

                                return result;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                NAPI.Util.ConsoleOutput($"QueryRead({command.CommandText}) Exception: {e.ToString()}");
                return null;
            }
        }
    }
}
