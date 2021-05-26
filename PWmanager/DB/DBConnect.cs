using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Text;
using MySql.Data.MySqlClient;

namespace PWmanager.DB
{
    using Model;

    class DBConnect : IDisposable
    {
        private string connectionString;
        private MySqlConnection _connection = null;

        public DBConnect()
        {
            connectionString = Properties.Settings.Default.ConnectionString;
        }

        #region Connections
        public bool Open()
        {
            try
            {
                _connection = new MySqlConnection(connectionString);
                _connection.Open();
                return true;
            }
            catch(Exception ex)
            {
                Trace.WriteLine(ex.StackTrace);
            }
            return false;
        }

        public bool Close()
        {
            try
            {
                if (_connection != null)
                {
                    _connection.Close();
                    _connection = null;
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void Dispose()
        {
            Close();
        }
        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        
        public bool Login(string email, string password, out User user)
        {
            bool status = false;
            User _user = null;
            if (Open())
            {
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM users WHERE email=@1 AND password=SHA2(@2, 224)", _connection);
                cmd.Parameters.AddWithValue("@1", email);
                cmd.Parameters.AddWithValue("@2", password);
                cmd.Prepare();

                MySqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    _user = new User(rdr.GetInt32("id"), rdr.GetString("name"), rdr.GetString("email"));
                    status = true;
                }

                rdr.Close();
                Close();          
            }

            user = _user;
            return status;
        }


        public List<User> GetUsers()
        {
            List<User> list = new List<User>();

            if (Open())
            {
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM users", _connection);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    list.Add(new User(rdr.GetInt32("id"), rdr.GetString("name"), rdr.GetString("email")));
                }

                rdr.Close();
                Close();
            }

            return list;
        }

        public List<Account> GetAccounts()
        {
            List<Account> list = new List<Account>();

            if (Open())
            {
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM accounts", _connection);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    list.Add(new Account(
                        rdr.GetInt32("id"),
                        rdr.GetInt32("group_id"),
                        rdr.GetInt32("added_by"),
                        rdr.GetString("email"),
                        rdr.GetString("password"),
                        rdr.GetDateTime("created_at")));
                }

                rdr.Close();
                Close();
            }

            return list;
        }
        
        public List<Account> GetAccountsBy(AccountGroup accountGroup)
        {
            List<Account> list = new List<Account>();

            if (Open())
            {
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM accounts WHERE group_id=@1", _connection);
                cmd.Parameters.AddWithValue("@1", accountGroup.ID);
                cmd.Prepare();
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    list.Add(new Account(
                        rdr.GetInt32("id"),
                        rdr.GetInt32("group_id"),
                        rdr.GetInt32("added_by"),
                        rdr.GetString("email"),
                        rdr.GetString("password"),
                        rdr.GetDateTime("created_at")));
                }

                rdr.Close();
                Close();
            }

            return list;
        }


        public List<AccountGroup> GetAccountGroups()
        {
            List<AccountGroup> list = new List<AccountGroup>();

            if (Open())
            {
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM account_groups ORDER BY name ASC", _connection);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    list.Add(new AccountGroup(
                        rdr.GetInt32("ID"),
                        rdr.GetString("name"),
                        rdr.GetString("description"),
                        rdr.GetDateTime("created_at")
                        ));
                }

                rdr.Close();
                Close();
            }

            return list;
        }
    }


}
