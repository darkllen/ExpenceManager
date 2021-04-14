using System.Threading.Tasks;
using DataStorage;
using ExpenceManagerModels.Users;

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
