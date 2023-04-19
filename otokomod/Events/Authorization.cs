using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using GTANetworkAPI;
using otokomod.MySQL;

namespace otokomod.Events.Events
{
    internal class Authorization : Script
    {
        [RemoteEvent("authOnRegister")]
        private void OnRegister(Player player, string firstName, string lastName, string email, string password)
        {
            if (PlayersAccountsDB.IsAccountExist(player.Name))
            {
                NAPI.ClientEvent.TriggerClientEvent(player, "showTextError", "An account with this nickname already exists.");
                return;
            }

            Accounts.Register(player, firstName, lastName, email, password);
            NAPI.ClientEvent.TriggerClientEvent(player, "closeAuthWindow");
            NAPI.ClientEvent.TriggerClientEvent(player, "PlayerFreeze", false);

        }

        [RemoteEvent("authOnLogin")]
        private void OnLogin(Player player, string email, string password)
        {
            if (!PlayersAccountsDB.IsAccountExist(player.Name))
            {
                NAPI.ClientEvent.TriggerClientEvent(player, "showTextError", "You are not registered.");
                return;
            }

            if (!PlayersAccountsDB.IsValidPassword(email, password)) 
            {
                NAPI.ClientEvent.TriggerClientEvent(player, "showTextError", "Wrong password.");
                return;
            }

            Accounts.Login(player, false);
            NAPI.ClientEvent.TriggerClientEvent(player, "closeAuthWindow");
            NAPI.ClientEvent.TriggerClientEvent(player, "PlayerFreeze", false);
        }
    }

    
}
