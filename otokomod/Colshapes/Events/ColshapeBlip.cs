using GTANetworkAPI;
using MySql.Data.MySqlClient;
using otokomod.MySQL;
using System;
using System.Data;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;

namespace otokomod.Colshapes.Events
{
    internal class ColshapeBlip
    {
        public static void Blip(ColShape shape, Player player)
        {
            bool transparent = true;
            player.SendChatMessage($"You entered {shape.Id}.");

            using MySqlCommand cmd = new MySqlCommand()
            {
                CommandText = $"SELECT * FROM {player.Name} WHERE id={shape.Id + 1}"
            };

            using DataTable result = DB.QueryRead("players_blips", cmd);

            if (result != null && result.Rows.Count >= 1)
            {
                var dateRow = result.Rows[0];
                transparent = Convert.ToBoolean(dateRow["transparent"]);
            }


            if (!transparent)
            {
                using MySqlCommand cmd2 = new MySqlCommand()
                {
                    CommandText = $"SELECT * FROM map_blips WHERE id={shape.Id + 1}"
                };

                using DataTable result2 = DB.QueryRead("main", cmd2);

                if (result2 != null && result2.Rows.Count >= 1)
                {
                    var dateRow = result2.Rows[0];
                    transparent = Convert.ToBoolean(dateRow["transparent"]);
                    int sprite = Convert.ToInt32(dateRow["sprite"]);
                    int X = Convert.ToInt32(dateRow["x"]);
                    int Y = Convert.ToInt32(dateRow["y"]);
                    int Z = Convert.ToInt32(dateRow["z"]);
                    float scale = (float)Convert.ToDouble(dateRow["scale"]);
                    int color = Convert.ToInt32(dateRow["color"]);
                    string name = dateRow["name"].ToString();

                    NAPI.ClientEvent.TriggerClientEvent(player, "blip", sprite, new Vector3(X, Y, Z), scale, (byte)color, name);
                }

                using MySqlCommand cmd3 = new MySqlCommand
                {
                    CommandText = $"UPDATE {player.Name} SET transparent = true WHERE id={shape.Id + 1}"
                };
                DB.Query("players_blips", cmd3);
            }

        }
    }
}
