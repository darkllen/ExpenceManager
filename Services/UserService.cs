using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStorage;
using ExpenceManager;
using ExpenceManagerModels.Users;
using ExpenceManagerModels.Wallet;

namespace Services
{
    public class UserService
    {
        private FileDataStorage<UserDB> _storage = new FileDataStorage<UserDB>();

        public async Task<bool> recordCategories(User user)
        {
            UserDB userDb = await _storage.GetAsync(user.Guid);
            userDb.Categories = user.Categories;
            await _storage.AddOrUpdateAsync(userDb);
            return true;
        }
    }
}
