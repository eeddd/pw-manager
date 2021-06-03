using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace PWmanager.Model
{
    public class Account : IModel
    {
        public int ID { get; set; }

        public int GroupID { get; set; }

        public int AddedBy { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Notes { get; set; }

        public DateTime CreatedAt { get; set; }


    }
}
