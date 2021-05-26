﻿using System;
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

        #region Auth
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
                    CurrentUser.Set(_user);
                    status = true;
                }

                rdr.Close();
                Close();          
            }

            user = _user;
            return status;
        }
        #endregion

        #region User
        public User ReadUser(MySqlDataReader rdr)
        {
            return new User(rdr.GetInt32("id"), rdr.GetString("name"), rdr.GetString("email"));
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
                    list.Add(ReadUser(rdr));
                }

                rdr.Close();
                Close();
            }

            return list;
        }
        #endregion

        #region Account
        public Account ReadAccount(MySqlDataReader rdr)
        {
            return new Account(
                        rdr.GetInt32("id"),
                        rdr.GetInt32("group_id"),
                        rdr.GetInt32("added_by"),
                        rdr.GetString("name"),
                        rdr.GetString("email"),
                        rdr.GetString("password"),
                        rdr.GetString("notes"),
                        rdr.GetDateTime("created_at"));
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
                    list.Add(ReadAccount(rdr));
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
                    list.Add(ReadAccount(rdr));
                }

                rdr.Close();
                Close();
            }

            return list;
        }

        public int InsertAccount(Account account)
        {
            int rows = 0; // affected rows
            try
            {
                if (Open())
                {
                    using (var cmd = new MySqlCommand(
                        "INSERT INTO accounts ( group_id,  added_by,  name,  email,  password,  notes)" +
                                     " VALUES (@group_id, @added_by, @name, @email, @password, @notes)",
                        _connection))
                    {
                        cmd.Parameters.AddWithValue("@group_id", account.GroupID);
                        cmd.Parameters.AddWithValue("@added_by", account.AddedBy);
                        cmd.Parameters.AddWithValue("@name", account.Name);
                        cmd.Parameters.AddWithValue("@email", account.Email);
                        cmd.Parameters.AddWithValue("@password", account.Password);
                        cmd.Parameters.AddWithValue("@notes", account.Notes);
                        cmd.Prepare();
                        rows = cmd.ExecuteNonQuery();
                        Trace.WriteLine("Insert: affected rows " + rows);
                    }
                }
            }
            catch
            {
            }
            return rows;
        }
        #endregion

        #region AccountGroup
        private AccountGroup ReadAccountGroup(MySqlDataReader rdr)
        {
            return new AccountGroup(
                        rdr.GetInt32("ID"),
                        rdr.GetString("name"),
                        rdr.GetString("description"),
                        rdr.GetDateTime("created_at")
                        );
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
                    list.Add(ReadAccountGroup(rdr));
                }

                rdr.Close();
                Close();
            }

            return list;
        }
        #endregion

    }


}
