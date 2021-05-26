using System;
using System.Collections.Generic;
using System.Text;

namespace PWmanager.Model
{
    public class Account
    {
        public int ID { get; private set; }

        public int GroupID { get; private set; }

        public int AddedBy { get; private set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public DateTime CreatedAt { get; private set; }


        public Account(int id, int groupID, int addedBy, string email, string password, DateTime createdAt)
        {
            ID = id;
            GroupID = groupID;
            AddedBy = addedBy;
            Email = email;
            Password = password;
            CreatedAt = createdAt;
        }
    }
}
