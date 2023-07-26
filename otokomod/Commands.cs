using System;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using System.Text;
using GTANetworkAPI;
using MySqlX.XDevAPI;
using otokomod.MySQL;

namespace otokomod
{
    internal class Commands : Script
    {
        [Command("veh", "/Spawn auto", Alias = "vechicle")]
        private void cmd_veh(Player player, string vehname, int color1 = 1, int color2 = 1) 
        {
            uint vhash = NAPI.Util.GetHashKey(vehname);
            if(vhash <= 0 ) 
            {
                player.SendChatMessage("~r~non-existent model");
            }
            Vehicle veh = NAPI.Vehicle.CreateVehicle(vhash, player.Position, player.Heading, color1, color2);
            player.SetIntoVehicle(veh, (int)VehicleSeat.Driver);
        }

        [Command("freeze", "/Freeze player")]
        private void cmd_freezeplayer(Player player, Player target, bool freezestatus)
        {
            NAPI.ClientEvent.TriggerClientEvent(target, "PlayerFreeze", freezestatus);
        }

        [Command("RequestIpl", "/x y z IplName", Alias = "reqipl")]
        private void cmd_requestipl(Player player, int x, int y, int z, string ipl) 
        {
            NAPI.World.RequestIpl(ipl);
            player.Position = new Vector3(x, y, z);
        }

        [Command("RemoveIpl", "/IplName", Alias = "remipl")]
        private void cmd_removeipl(Player player, string ipl)
        {
            NAPI.World.RemoveIpl(ipl);
        }

        [Command("blip", "/blip")]
        private void cmd_blip(Player player, int sprite, float scale, int color, string name)
        {
            Vector3 position = player.Position;

            Blip blip = NAPI.Blip.CreateBlip(sprite, position, scale, (byte)color, name);
            NAPI.Blip.SetBlipShortRange(blip, true);

            player.SendChatMessage(position.X + " " + position.Y + " " + position.Z);

            MapBlipsDB.NewBlip(sprite, position, scale, color, name);
        }

        
    }
}
