using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using GTANetworkAPI;
using MySql.Data.MySqlClient;

namespace otokomod.MySQL
{
    internal class PlayersAccountsDB
    {
        public static bool IsAccountExist(string nickName)
        {
            MySqlCommand command = DB.connection.CreateCommand();
            command.CommandText = "SELECT * FROM players_accounts WHERE nickName=@nickName LIMIT 1";
            command.Parameters.AddWithValue("@nickName", nickName);

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    return true;
                }
                return false;
            }
        }

        public static void NewAccountRegister(Player player, string firstName, string lastName, string email, string password)
        {
            string saltPw = BCrypt.HashPassword(password, BCrypt.GenerateSalt());

            try
            {
                MySqlCommand command = DB.connection.CreateCommand();

                command.CommandText = "INSERT INTO players_accounts (pass, nickName, email, firstName, lastName) VALUES (@pass, @nickName, @email, @firstName, @lastName)";
                command.Parameters.AddWithValue("@pass", saltPw);
                command.Parameters.AddWithValue("@nickName", player.Name);
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@firstName", firstName);
                command.Parameters.AddWithValue("@lastName", lastName);

                command.ExecuteNonQuery();

                command.CommandText = $"CREATE TABLE {player.Name} AS SELECT * FROM map_blips";

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput("Exception: " + ex.ToString());
            }


        }

        public static void LoadAccount(Player player)
        {
            MySqlCommand command = DB.connection.CreateCommand();

            command.CommandText = "SELECT * FROM players_accounts WHERE nickName=@nickName LIMIT 1";
            command.Parameters.AddWithValue("@nickName", player.Name);

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    
                }

            }
        }

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

            MySqlCommand command = DB.connection.CreateCommand();
            command.CommandText = "SELECT pass FROM players_accounts WHERE email=@email LIMIT 1";
            command.Parameters.AddWithValue("@email", email);

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    tempPass = reader.GetString("pass");
                }
            }

            if (BCrypt.CheckPassword(inputPassword, tempPass)) return true;
            return false;
        }
    }
}
