using GTANetworkAPI;
//using GTANetworkMethods;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace otokomod.MySQL
{
    internal class MapBlipsDB
    {       

        
        public static void NewBlip(int sprite, Vector3 position, float scale, int color, string name)
        {
            try
            {
                MySqlCommand command = DB.connection.CreateCommand();

                command.CommandText = "INSERT INTO map_blips (sprite, x, y, z, scale, color, name) VALUES (@sprite, @x, @y, @z, @scale, @color, @name)";
                command.Parameters.AddWithValue("@sprite", sprite);
                command.Parameters.AddWithValue("@x", position.X);
                command.Parameters.AddWithValue("@y", position.Y);
                command.Parameters.AddWithValue("@z", position.Z);
                command.Parameters.AddWithValue("@scale", scale);
                command.Parameters.AddWithValue("@color", color);
                command.Parameters.AddWithValue("@name", name);

                command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput("Exception: " + ex.ToString());
            }
        }

        public static void LoadBlips()
        {
            MySqlCommand command = DB.connection.CreateCommand();

            command.CommandText = "SELECT * FROM map_blips";

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        /*Blip blip = NAPI.Blip.CreateBlip(reader.GetInt32("sprite"), 
                            new Vector3(reader.GetInt32("x"), reader.GetInt32("y"), reader.GetInt32("z")),
                            reader.GetFloat("scale"),
                            (byte)reader.GetInt32("color"),
                            reader.GetString("name"));

                        NAPI.Blip.SetBlipShortRange(blip, true);
                        NAPI.Blip.SetBlipTransparency(blip, 0); */

                        NAPI.ColShape.CreatCircleColShape(reader.GetInt32("x"), reader.GetInt32("y"), 10f);

                       
                    }
                }

            }
        }
    }
}
