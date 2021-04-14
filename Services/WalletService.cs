using System.Collections.Generic;
using System.Threading.Tasks;
using DataStorage;
using ExpenceManagerModels.Users;
using ExpenceManagerModels.Wallet;

namespace Services
{
    public class WalletService
    {
        private FileDataStorage<WalletDb> _storage = new FileDataStorage<WalletDb>();
        public static Wallet CurrentWallet; 


        public async Task<User> LoadUserWallets(User user)
        {
            var transactService = new TransactionService();
            List<WalletDb> wallets = await _storage.GetAllAsyncForObject(user);
            foreach (var wallet in wallets)
            {
               Wallet wallet_cr = Wallet.CreateWalletForUser(user, wallet.Name, wallet.CurrBalance, wallet.Description, wallet.Currency, wallet.Guid);
               await transactService.LoadWalletTransactions(wallet_cr);

            }
            return user;
        }

        public async Task<bool> SaveUpdateWallet(User user, WalletDb wallet)
        {
            await _storage.AddOrUpdateAsyncForObject(wallet, user);
            return true;
        }

        public async Task<bool> RemoveWallet(WalletDb wallet)
        {
            var transactService = new TransactionService();

            await _storage.RemoveObj(wallet);
            return true;
        }
    }
}
