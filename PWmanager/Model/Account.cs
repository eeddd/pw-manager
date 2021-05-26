using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace PWmanager.Model
{
    public class Account : IModel
    {
        public int ID { get; private set; }

        public int GroupID { get; private set; }

        public int AddedBy { get; private set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Notes { get; set; }

        public DateTime CreatedAt { get; private set; }


        public Account(int id, int groupID, int addedBy, string name,string email, string password, string notes, DateTime createdAt)
        {
            ID = id;
            GroupID = groupID;
            AddedBy = addedBy;
            Name = name;
            Email = email;
            Password = password;
            Notes = notes;
            CreatedAt = createdAt;
        }


    }
}
