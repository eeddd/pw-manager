using System;
using System.Collections.Generic;
using System.Text;

namespace PWmanager.Model
{
    public class AccountGroup
    {
        public int ID { get; private set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime CreatedAt { get; private set; }


        public AccountGroup(int id, string groupName, string description, DateTime createdAt)
        {
            ID = id;
            Name = groupName;
            Description = description;
            CreatedAt = createdAt;
        }
    }
}
