using GTANetworkAPI;
using MySql.Data.MySqlClient;
using otokomod.MySQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection.PortableExecutable;
using System.Text;

namespace otokomod.Events.Client
{
    internal class EventsTX
    {
        public static void Blips(Player player)
        {
            using MySqlCommand cmd = new MySqlCommand()
            {
                CommandText = $"SELECT * FROM {player.Name} WHERE transparent = TRUE"
            };

            using DataTable result = DB.QueryRead("players_blips", cmd);

            if (result != null && result.Rows.Count >= 1)
            {
                var dateRow = result.Rows[0];
                int sprite = Convert.ToInt32(dateRow["sprite"]);
                int X = Convert.ToInt32(dateRow["x"]);
                int Y = Convert.ToInt32(dateRow["y"]);
                int Z = Convert.ToInt32(dateRow["z"]);
                float scale = (float)Convert.ToDouble(dateRow["scale"]);
                int color = Convert.ToInt32(dateRow["color"]);
                string name = dateRow["name"].ToString();

                NAPI.ClientEvent.TriggerClientEvent(player, "blip", sprite, new Vector3(X, Y, Z), scale, (byte)color, name);
            }
        }


    }
}
