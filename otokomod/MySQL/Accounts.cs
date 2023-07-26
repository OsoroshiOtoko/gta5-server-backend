using GTANetworkAPI;

namespace otokomod.MySQL
{
    class Accounts : PlayersAccountsDB
    {       
        public static void Register(Player player, string firstName, string lastName, string email, string password)
        {
            NewAccountRegister(player, firstName, lastName, email, password);
            Login(player, true);
        }

        public static void Login(Player player, bool isFirstLogin)
        {
            AccountData(player);

            if (isFirstLogin)
            {
                player.SendChatMessage("You have successfully registered!");
            }

            player.SendChatMessage("You are successfully authorized!");

        }
    }
}
