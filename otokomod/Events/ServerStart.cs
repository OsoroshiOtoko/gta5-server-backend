using System;
using GTANetworkAPI;
using otokomod.MySQL;
using System.Drawing.Printing;
using MySqlX.XDevAPI;
using MySql.Data.MySqlClient;
using System.Security.Principal;

namespace otokomod.Events
{
    class ServerStart : Script
    {
        [ServerEvent(Event.ResourceStart)]
        public void OnResourceStarted()
        {
            DB.InitConnection();
            MapBlipsDB.LoadBlips();
        }

        [ServerEvent(Event.PlayerConnected)]
        private void OnPlayerConnected(Player player)
        {
            if (PlayersAccountsDB.IsAccountExist(player.Name))
            {
                MySqlCommand command = DB.connection.CreateCommand();
                command.CommandText = $"SELECT * FROM {player.Name} WHERE transparent = TRUE";



                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            NAPI.ClientEvent.TriggerClientEvent(player, "NewBlip", reader.GetInt32("sprite"),
                                new Vector3(reader.GetInt32("x"), reader.GetInt32("y"), reader.GetInt32("z")),
                                reader.GetFloat("scale"),
                                (byte)reader.GetInt32("color"),
                                reader.GetString("name"));
                        }
                    }
                }
            }
        }


        [ServerEvent(Event.PlayerSpawn)]
        private void OnPlayerSpawn(Player player)
        {
            player.Position = new Vector3(-535, -280, 38.2);

            player.Health = 50;
            player.Armor = 50;
        }


        [ServerEvent(Event.PlayerEnterColshape)]
        public void OnPlayerEnterColshape(ColShape shape, Player player)
        {
            bool transparent = true;
            player.SendChatMessage($"You entered {shape.Id}.");

            MySqlCommand command = DB.connection.CreateCommand();

            command.CommandText = $"SELECT * FROM {player.Name} WHERE id={shape.Id + 1}";

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    transparent = reader.GetBoolean("transparent");
                }

            }

            if (!transparent)
            {
                command.CommandText = $"SELECT * FROM map_blips WHERE id={shape.Id + 1}";

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            NAPI.ClientEvent.TriggerClientEvent(player, "NewBlip", reader.GetInt32("sprite"),
                                new Vector3(reader.GetInt32("x"), reader.GetInt32("y"), reader.GetInt32("z")),
                                reader.GetFloat("scale"),
                                (byte)reader.GetInt32("color"),
                                reader.GetString("name"));
                        }
                    }
                }

                command.CommandText = $"UPDATE {player.Name} SET transparent = true WHERE id={shape.Id + 1}";
                command.ExecuteNonQuery();
            }
        }
    }
}
