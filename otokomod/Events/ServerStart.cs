using System;
using GTANetworkAPI;
using otokomod.MySQL;
using otokomod.Events.Client;
using otokomod.Colshapes.Events;
using System.Data;

namespace otokomod.Events
{
    class ServerStart : Script
    {
        [ServerEvent(Event.ResourceStart)]
        public void OnResourceStarted()
        {
            DB.Test("main");
            DB.Test("players_blips");
            MapBlipsDB.LoadBlips();
            Ped ped = NAPI.Ped.CreatePed(0x2F8845A3, new Vector3(-535, -280, 37), 1f,true, false, false, false);
        }

        [ServerEvent(Event.PlayerConnected)]
        private void OnPlayerConnected(Player player)
        {
            DataTable data = PlayersAccountsDB.AccountData(player);
            if (data != null && data.Rows.Count >= 1)
            {
                var dateRow = data.Rows[0];
                NAPI.ClientEvent.TriggerClientEvent(player, "loginData", dateRow["email"].ToString());
                EventsTX.Blips(player);
            }
        }


        [ServerEvent(Event.PlayerSpawn)]
        private void OnPlayerSpawn(Player player)
        {
            player.Position = new Vector3(-530, -280, 37);

            player.Health = 50;
            player.Armor = 50;
        } 


        [ServerEvent(Event.PlayerEnterColshape)]
        public void OnPlayerEnterColshape(ColShape shape, Player player)
        {
            ColshapeBlip.Blip(shape, player);
            
        }
    }
}
