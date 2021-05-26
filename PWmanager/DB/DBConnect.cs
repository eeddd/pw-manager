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


        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        public void Insert()
        {

        }

        public void Update()
        {

        }

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


    }


}
