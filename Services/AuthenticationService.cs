using System;
using System.Threading;
using System.Threading.Tasks;
using ExpenceManager;

namespace Services
{
    public class AuthenticationService
    {
        //private FileDataStorage<DBUser> _storage = new FileDataStorage<DBUser>();

        public async Task<User> AuthenticateAsync(User authUser)
        {
            // if (String.IsNullOrWhiteSpace(authUser.Login) || String.IsNullOrWhiteSpace(authUser.Password))
            //     throw new ArgumentException("Login or Password is Empty");
            // var users = await _storage.GetAllAsync();
            // var dbUser = users.FirstOrDefault(user => user.Login == authUser.Login && user.Password == authUser.Password);
            // if (dbUser == null)
            //     throw new Exception("Wrong Login or Password");
            return new User("Ihor", "Yankin", "aaa@aaa");
        }

        public async Task<bool> RegisterUserAsync(User regUser)
        {
            Thread.Sleep(2000);
            // var users = await _storage.GetAllAsync();
            // var dbUser = users.FirstOrDefault(user => user.Login == regUser.Login);
            // if (dbUser != null)
            //     throw new Exception("User already exists");
            // if (String.IsNullOrWhiteSpace(regUser.Login) || String.IsNullOrWhiteSpace(regUser.Password) || String.IsNullOrWhiteSpace(regUser.LastName))
            //     throw new ArgumentException("Login, Password or Last Name is Empty");
            // dbUser = new DBUser(regUser.LastName + "First", regUser.LastName, regUser.Login + "@gmail.com",
            //     regUser.Login, regUser.Password);
            // await _storage.AddOrUpdateAsync(dbUser);
            return true;
        }
    }
}
