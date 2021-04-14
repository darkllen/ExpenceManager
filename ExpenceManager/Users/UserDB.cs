using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using DataStorage;

namespace ExpenceManagerModels.Users
{
    public class UserDb : IStorable
    {
        public UserDb(string login, string password, string name, string surname, string email)
        {
            Guid = Guid.NewGuid();
            Login = login;
            Password = password;
            Name = name;
            Surname = surname;
            Email = email;
        }

        [JsonConstructor]
        public UserDb(Guid guid, string login, string password, string name, string surname, string email, List<Category> categories)
        {
            Guid = guid;
            Login = login;
            Password = password;
            Name = name;
            Surname = surname;
            Email = email;
            Categories = categories;
        }

        public Guid Guid { get; }
        public string Login { get; set; }
        public string Password { get; set; }

        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }

        public List<Category> Categories { get; set; }
    }
}
