﻿using System;
using MySql.Data.MySqlClient;

namespace DesignPatterns.MidSemester
{
    public class Preference
    {
        // TODO: implement

        private static Preference instance;

        string serverIp = "localhost";
        string username = "root";
        string password = "rootroot";
        string databaseName = "assignment";

        MySqlConnection connection;
        MySqlCommand mySqlCommand;

        private Preference()
        {
            string dbConnectionString = string.Format("server={0};uid={1};pwd={2};database={3};", serverIp, username, password, databaseName);
            this.connection = new MySqlConnection(dbConnectionString);
            connection.Open();
        }
        public static Preference getInstance()
        {
            if (instance == null)
            {
                //acquireThreadLock();
                if (instance == null)
                {
                    instance = new Preference();
                }
            }
            return instance;
        }


        public void setPreference(string key, string val)
        {
            string query = "INSERT INTO pref(keykey, value) VALUES(@key, @value)";

            MySqlCommand mySqlCommand = new MySqlCommand(query, this.connection);
            mySqlCommand.Parameters.AddWithValue("@key", key);
            mySqlCommand.Parameters.AddWithValue("@value", val);
            try
            {
                mySqlCommand.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                if (e.Number == 1062)
                {
                    query = "UPDATE pref SET value = @value WHERE keykey = @key ";
                    mySqlCommand = new MySqlCommand(query, this.connection);
                    mySqlCommand.Parameters.AddWithValue("@key", key);
                    mySqlCommand.Parameters.AddWithValue("@value", val);

                    mySqlCommand.ExecuteNonQuery();
                }
                Console.WriteLine("DatabaseName Error : " + e);
            }

        }

        public string getPreference(string key)
        {

            string value = null;
            String query = "Select * from pref WHERE keykey= @key";
            mySqlCommand = new MySqlCommand(query, this.connection);
            mySqlCommand.Parameters.AddWithValue("@key", key);
            var reader = mySqlCommand.ExecuteReader();

            while (reader.Read())
            {
                value = reader["value"].ToString();
            }
            reader.Close();
            return value;
        }

    }

}
