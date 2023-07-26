using System.Data;
using GTANetworkAPI;
using MySql.Data.MySqlClient;

namespace otokomod.MySQL
{
    internal class PlayersAccountsDB
    {
        public static DataTable AccountData(Player player)
        {
            using MySqlCommand cmd = new MySqlCommand()
            {
                CommandText = "SELECT * FROM players_accounts WHERE nickName=@nickName LIMIT 1"
            };
            cmd.Parameters.AddWithValue("@nickName", player.Name);

            using DataTable result = DB.QueryRead("main", cmd);

            return result;

        }

        public static void NewAccountRegister(Player player, string firstName, string lastName, string email, string password)
        {
            string saltPw = BCrypt.HashPassword(password, BCrypt.GenerateSalt());

            using MySqlCommand cmd = new MySqlCommand
            {
                CommandText = "INSERT INTO players_accounts (pass, nickName, email, firstName, lastName) VALUES (@pass, @nickName, @email, @firstName, @lastName)"
            };
            cmd.Parameters.AddWithValue("@pass", saltPw);
            cmd.Parameters.AddWithValue("@nickName", player.Name);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@firstName", firstName);
            cmd.Parameters.AddWithValue("@lastName", lastName);

            DB.Query("main", cmd);
        }

        /*public static void LoadAccount(Player player)
        {
            using MySqlCommand cmd = new MySqlCommand()
            {
                CommandText = "SELECT * FROM players_accounts WHERE nickName=@nickName LIMIT 1"
            };
            cmd.Parameters.AddWithValue("@nickName", player.Name);

            using DataTable result = DB.QueryRead("main", cmd);
        }*/

        /*public static void SaveAccount(Accounts account)
        {
            MySqlCommand command = DB.connection.CreateCommand();

            command.CommandText = "UPDATE players_accounts SET cash=@cash WHERE id=@id";
            command.Parameters.AddWithValue("@cash", 1000);
            command.Parameters.AddWithValue("@id", account._id);
        } */

        public static bool IsValidPassword(string email, string inputPassword)
        {
            string tempPass = " ";

            using MySqlCommand cmd = new MySqlCommand()
            {
                CommandText = "SELECT pass FROM players_accounts WHERE email=@email LIMIT 1"
            };
            cmd.Parameters.AddWithValue("@email", email);

            using DataTable result = DB.QueryRead("main", cmd);

            if (result != null && result.Rows.Count >= 1)
            {
                var dateRow = result.Rows[0];
                tempPass = dateRow["pass"].ToString();
            }

            if (BCrypt.CheckPassword(inputPassword, tempPass)) return true;
            return false;
        }
    }
}
