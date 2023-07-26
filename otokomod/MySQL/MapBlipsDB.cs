using GTANetworkAPI;
using GTANetworkMethods;
//using GTANetworkMethods;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection.PortableExecutable;
using System.Text;

namespace otokomod.MySQL
{
    internal class MapBlipsDB
    {       

        
        public static void NewBlip(int sprite, Vector3 position, float scale, int color, string name)
        {
            using MySqlCommand cmd = new MySqlCommand
            {
                CommandText = "INSERT INTO map_blips (sprite, x, y, z, scale, color, name) VALUES (@sprite, @x, @y, @z, @scale, @color, @name)"
            };
            cmd.Parameters.AddWithValue("@sprite", sprite);
            cmd.Parameters.AddWithValue("@x", position.X);
            cmd.Parameters.AddWithValue("@y", position.Y);
            cmd.Parameters.AddWithValue("@z", position.Z);
            cmd.Parameters.AddWithValue("@scale", scale);
            cmd.Parameters.AddWithValue("@color", color);
            cmd.Parameters.AddWithValue("@name", name);

            DB.Query("main", cmd);
        }

        public static void LoadBlips()
        {
            int X, Y;

            using MySqlCommand cmd = new MySqlCommand()
            {
                CommandText = "SELECT * FROM map_blips"
            };

            using DataTable result = DB.QueryRead("main", cmd);

            if (result != null && result.Rows.Count >= 1)
            {
                var dateRow = result.Rows[0];
                X = Convert.ToInt32(dateRow["x"]);
                Y = Convert.ToInt32(dateRow["y"]);

                /*Blip blip = NAPI.Blip.CreateBlip(reader.GetInt32("sprite"), 
                            new Vector3(reader.GetInt32("x"), reader.GetInt32("y"), reader.GetInt32("z")),
                            reader.GetFloat("scale"),
                            (byte)reader.GetInt32("color"),
                            reader.GetString("name"));

                        NAPI.Blip.SetBlipShortRange(blip, true);
                        NAPI.Blip.SetBlipTransparency(blip, 0); */

                NAPI.ColShape.CreatCircleColShape(X, Y, 10f);
            }
        }
    }
}
