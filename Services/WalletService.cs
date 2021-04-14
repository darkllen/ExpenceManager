using System.Collections.Generic;
using System.Threading.Tasks;
using DataStorage;
using ExpenceManager;
using ExpenceManagerModels.Wallet;

namespace Services
{
    public class WalletService
    {
        private FileDataStorage<WalletDB> _storage = new FileDataStorage<WalletDB>();
        public static Wallet CurrentWallet; 


        public async Task<User> LoadUserWallets(User user)
        {
            var transactService = new TransactionService();
            List<WalletDB> wallets = await _storage.GetAllAsyncForObject(user);
            foreach (var wallet in wallets)
            {
               Wallet wallet_cr = Wallet.CreateWalletForUser(user, wallet.Name, wallet.CurrBalance, wallet.Description, wallet.Currency, wallet.Guid);
               await transactService.LoadWalletTransactions(wallet_cr);

            }
            return user;
        }

        public async Task<bool> SaveUpdateWallet(User user, WalletDB wallet)
        {
            await _storage.AddOrUpdateAsyncForObject(wallet, user);
            return true;
        }

        public async Task<bool> RemoveWallet(WalletDB wallet)
        {
            var transactService = new TransactionService();

            await _storage.RemoveObj(wallet);
            return true;
        }
    }
}
