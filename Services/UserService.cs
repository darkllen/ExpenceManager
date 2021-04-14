using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStorage;
using ExpenceManagerModels.Users;
using ExpenceManagerModels.Wallet;

namespace Services
{
    public class UserService
    {
        private FileDataStorage<UserDb> _storage = new FileDataStorage<UserDb>();

        public async Task<bool> RecordCategories(User user)
        {
            UserDb userDb = await _storage.GetAsync(user.Guid);
            userDb.Categories = user.Categories;
            await _storage.AddOrUpdateAsync(userDb);
            return true;
        }
    }
}
