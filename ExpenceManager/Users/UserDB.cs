using System;
using DataStorage;

namespace ExpenceManagerModels.Users
{
    public class UserDB : IStorable
    {
        public UserDB(string login, string password, string name, string surname, string email)
        {
            Guid = Guid.NewGuid();
            Login = login;
            Password = password;
            Name = name;
            Surname = surname;
            Email = email;
        }

        public Guid Guid { get; }
        public string Login { get; set; }
        public string Password { get; set; }

        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
    }
}
