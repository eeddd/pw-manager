using System;
using System.Collections.Generic;
using System.Text;

namespace PWmanager.Model
{
    public class User
    {
        public int ID { get; private set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public DateTime CreatedAt { get; private set; }


        public User(int ID, string name, string email)
        {
            this.ID = ID;
            Email = email;
            Name = name;
        }
    }

    public class CurrentUser
    {
        private static User _user = null;

        public static void Set(User user)
        {
            _user = user;
        }

        public static User Get() => _user;
    }

}
