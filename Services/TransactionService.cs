using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStorage;
using ExpenceManagerModels;
using ExpenceManagerModels.Wallet;

namespace Services
{
    public class TransactionService
    {
        private FileDataStorage<Transaction> _storage = new FileDataStorage<Transaction>();

        public async Task<Wallet> LoadWalletTransactions(Wallet wallet)
        {
            List<Transaction> transactions = await _storage.GetAllAsyncForObject(wallet);
            foreach (var transaction in transactions)
            {
                wallet.AddTransaction(AuthenticationService.CurrentUser, transaction);
            }
            return wallet;
        }

        public async Task<bool> SaveUpdateTransaction(Wallet wallet, Transaction transaction)
        {
            await _storage.AddOrUpdateAsyncForObject(transaction, wallet);
            return true;
        }

        public async Task<bool> RemoveTransaction(Transaction transaction)
        {
            await _storage.RemoveObj(transaction);
            return true;
        }
    }
}
