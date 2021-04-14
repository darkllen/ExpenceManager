using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataStorage;
using ExpenceManagerModels;
using ExpenceManagerModels.Users;

namespace Services
{
    public class AuthenticationService
    {
        private readonly FileDataStorage<UserDb> _storage = new FileDataStorage<UserDb>();
        public static User CurrentUser;

        public async Task<User> AuthenticateAsync(UserAuth authUser)
        {
            if (String.IsNullOrWhiteSpace(authUser.Login) || String.IsNullOrWhiteSpace(authUser.Password))
                throw new ArgumentException("Login or Password is Empty");
            var users = await _storage.GetAllAsync();
            UserDb dbUser = null;
            try
            {
                dbUser = users.FirstOrDefault(user =>
                    user.Login == authUser.Login &&
                    Encryption.Decrypt(user.Password, authUser.Password) == authUser.Password);
                if (dbUser == null)
                    throw new Exception();
            }
            catch (Exception)
            {
                throw new UserException("Wrong Login or Password");
            }

            dbUser.Categories ??= new List<Category>();

            CurrentUser = new User(dbUser.Name, dbUser.Surname, dbUser.Email, dbUser.Guid, dbUser.Categories);
            return CurrentUser;
        }

        public async Task<bool> RegisterUserAsync(UserReg regUser)
        {
            var users = await _storage.GetAllAsync();
            var dbUser = users.FirstOrDefault(user => user.Login == regUser.Login);
            if (dbUser != null)
                throw new UserException("User already exists");
            if (String.IsNullOrWhiteSpace(regUser.Login) ||
                String.IsNullOrWhiteSpace(regUser.Name) ||
                String.IsNullOrWhiteSpace(regUser.Surname) ||
                String.IsNullOrWhiteSpace(regUser.Password) || 
                String.IsNullOrWhiteSpace(regUser.Email))
                throw new ArgumentException("Login, Password, Name, Surname or Email is Empty");
            dbUser = new UserDb(regUser.Login, Encryption.Encrypt(regUser.Password, regUser.Password), regUser.Name, regUser.Surname, regUser.Email);
            await _storage.AddOrUpdateAsync(dbUser);
            return true;
        }
    }
}
