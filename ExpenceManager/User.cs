using System;
using System.Collections.Generic;

namespace ExpenceManager
{
    public class User
    {
        //user can't change name or surname
        public string Name { get; }
        public string Surname { get; }
        //user can change email
        public string Email { get; set; }
        //wallets are private, so User methods are needed to be used to work with wallets
        private List<Wallet> Wallets { get; }
        private List<Wallet> WalletsShared { get; }
        //categories are public, so we can change them easy
        public List<Category> Categories { get; }

        public User(string name, string surname, string email)
        {   
            Name = name;
            Surname = surname;
            Email = email;
            Wallets = new List<Wallet>();
            WalletsShared = new List<Wallet>();
            Categories = new List<Category>();
        }

        /// <summary>
        /// Create new wallet for User. 
        /// This is the only way to create wallet.
        /// </summary>
        /// <param name="name">wallet name</param>
        /// <param name="startBalance">wallet start ballance</param>
        /// <param name="description">wallet description</param>
        /// <param name="currency">wallet currency</param>
        /// <returns>id of created wallet</returns>
        public int CreateNewWallet(string name,
                                   decimal startBalance,
                                   string description,
                                   string currency)
        {
            //create wallet and asign it to user
            Wallet wallet = new Wallet(name, startBalance, description, currency, Categories);
            Wallets.Add(wallet);
            return wallet.Id;
        }

        /// <summary>
        /// get all wallets ids of user to use them later in User methods
        /// </summary>
        /// <returns>List with ids of wallets, which are owned by user</returns>
        public List<int> GetWalletsIds()
        {
            return ConvertWalletsIds(Wallets);
        }
        /// <summary>
        /// get all shared wallets ids of user to use them later in User methods
        /// </summary>
        /// <returns>List with ids of wallets, which are shared with user</returns>
        public List<int> GetSharedWalletsIds()
        {
            return ConvertWalletsIds(WalletsShared);
        }

        /// <summary>
        /// share wallet with another user
        /// </summary>
        /// <param name="walletId">wallet id</param>
        /// <param name="user">user to share wallet with</param>
        public void ShareWalletWithUser(int walletId, User user)
        {
            Wallet toShare = GetOwnWalletById(walletId);

            if (user == this) throw new UserException("can not share wallet with owner");
            if (toShare is null) throw new UserException("user hasn't this wallet");
            if (user.WalletsShared.Contains(toShare)) throw new UserException("wallet has been already shared with this user");

            user.WalletsShared.Add(toShare);
        }

        /// <summary>
        /// change categories accessebility for wallet.
        /// </summary>
        /// <param name="walletId">wallet id</param>
        /// <param name="category">category to change</param>
        public void SwitchCategoryPermisiion(int walletId, Category category)

        {
            Wallet wallet = GetOwnWalletById(walletId);

            if (!Categories.Contains(category)) throw new UserException("user hasn't this category");
            if (wallet is null) throw new UserException("user hasn't this wallet");

            if (wallet.RestrictedCategories.Contains(category)) wallet.RestrictedCategories.Remove(category);
            else wallet.RestrictedCategories.Add(category);
        }

        /// <summary>
        /// add transaction to wallet
        /// </summary>
        /// <param name="walletId">wallet id</param>
        /// <param name="transaction">transaction to add</param>
        public void AddTransaction(int walletId, Transaction transaction)
            
        {
            GetWalletOrException(walletId).AddTransaction(transaction);
        }

        /// <summary>
        /// remove transaction from wallet
        /// </summary>
        /// <param name="walletId">wallet id</param>
        /// <param name="transaction">transaction to remove</param>
        public void RemoveTransaction(int walletId, Transaction transaction)
        {
            GetWalletOrException(walletId).RemoveTransaction(transaction);
        }

        /// <summary>
        /// get 10 first transactions starts from certain position
        /// </summary>
        /// <param name="walletId">wallet id</param>
        /// <param name="start">position to start from</param>
        /// <returns>List with 10 or less transactions</returns>
        public List<Transaction> Get10Transactions(int walletId, int start)
        {
            return GetWalletOrException(walletId).Get10Transactions(start);
        }

        /// <summary>
        /// show wallet current balance
        /// </summary>
        /// <param name="walletId">wallet id</param>
        /// <returns>curent balance of wallet</returns>
        public decimal GetWalletBallance(int walletId)
        {
            return GetWalletOrException(walletId).CurrBalance;
        }

        /// <summary>
        /// get profit for this wallet started from the first day of current month
        /// </summary>
        /// <param name="walletId">walet id</param>
        /// <returns>amount of profit</returns>
        public decimal GetThisMonthProfit(int walletId)
        {
            return GetWalletOrException(walletId).MonthProfit();
        }

        /// <summary>
        /// get spends for this wallet started from the first day of current month
        /// </summary>
        /// <param name="walletId">wallet id</param>
        /// <returns>amount of spends</returns>
        public decimal GetThisMonthSpends(int walletId)
        {
            return GetWalletOrException(walletId).MonthSpends();
        }



        /// <summary>
        /// convert List of Wallet to List of Wallet.id
        /// </summary>
        /// <param name="wallets">List of wallets></param>
        /// <returns>List of ids</returns>
        private List<int> ConvertWalletsIds(List<Wallet> wallets)
        {
            static int map(Wallet x) => x.Id;
            return wallets.ConvertAll(map);
        }

        /// <summary>
        /// get Wallet from own wallets by id
        /// </summary>
        /// <param name="id">wallet id</param>
        /// <returns>wallet with this id</returns>
        private Wallet GetOwnWalletById(int id)
        {
            return GetWalletById(Wallets, id);
        }

        /// <summary>
        /// get Wallet from shared wallets by id
        /// </summary>
        /// <param name="id">wallet id</param>
        /// <returns>wallet with this id</returns>
        private Wallet GetSharedWalletById(int id)
        {
           return GetWalletById(WalletsShared, id);
        }

        /// <summary>
        /// find Wallet with certain id from List
        /// </summary>
        /// <param name="wallets">List of wallets</param>
        /// <param name="id">wallet id</param>
        /// <returns>wallet with this id</returns>
        private Wallet GetWalletById(List<Wallet> wallets, int id)
        {
            bool idPredicate(Wallet x) => x.Id == id;
            return wallets.Find(idPredicate);
        }

        /// <summary>
        /// search Wallet with id among all wallets of this user (own and shared)
        /// throws UserException id there is no such Wallet
        /// </summary>
        /// <param name="walletId">wallet id</param>
        /// <returns>wallet with this id</returns>
        private Wallet GetWalletOrException(int walletId)
        {
            Wallet wallet = GetOwnWalletById(walletId);
            if (wallet is null) wallet = GetSharedWalletById(walletId);
            if (wallet is null) throw new UserException("user hasn't this wallet");
            return wallet;
        }

        private class Wallet
        {
            // field to imitate autogenerated id
            private static int NextId = 0;
            public int Id { get; }
            private string Name { get; set; }
            private decimal StartBalance { get; }
            public decimal CurrBalance { get; set; }
            public string Description { get; set; }
            private string Currency { get; }

            private List<Transaction> Transactions;
            private List<Category> PossibleCategories;
            public List<Category> RestrictedCategories { get; }

            public Wallet(

                string name,
                decimal startBalance,
                string description,
                string currency,
                List<Category> possibleCategories)
            {
                Name = name;
                StartBalance = startBalance;
                Description = description;
                Currency = currency;
                //possibleCategories of Wallet are always actual as they changed with any changes in User categories
                PossibleCategories = possibleCategories;

                Id = NextId++;
                CurrBalance = startBalance;

                Transactions = new List<Transaction>();
                RestrictedCategories = new List<Category>();
            }

            /// <summary>
            /// get 10 first transactions starts from certain position
            /// </summary>
            /// <param name="startPlace">position to start from</param>
            /// <returns>List with 10 or less transactions</returns>
            public List<Transaction> Get10Transactions(int startPlace)
            {
                List<Transaction> returnedTransactions = new List<Transaction>();
                for (int i = startPlace; i < startPlace + 10 && i < Transactions.Count; i++)
                {
                    returnedTransactions.Add(Transactions[i]);
                }
                return returnedTransactions;
            }

            /// <summary>
            /// add transaction to wallet if possible
            /// </summary>
            /// <param name="transaction">transaction to add</param>
            public void AddTransaction(Transaction transaction)
            {
                if (Transactions.Contains(transaction)) throw new WalletException("this transaction already exists in this wallet");
                if (RestrictedCategories.Contains(transaction.Category)) throw new WalletException("transaction category is restricted for this wallet");
                if (!PossibleCategories.Contains(transaction.Category)) throw new WalletException("transaction category is not possible for this wallet");
                Transactions.Add(transaction);
                CurrBalance += transaction.Amount;
            }

            /// <summary>
            /// remove transaction from wallet
            /// </summary>
            /// <param name="transaction">transaction to remove</param>
            public void RemoveTransaction(Transaction transaction)
            {
                Transactions.Remove(transaction);
                CurrBalance -= transaction.Amount;
            }

            /// <summary>
            /// counting monthly spends
            /// </summary>
            /// <returns></returns>
            public decimal MonthSpends()
            {
                return CountingMonthStats(Stats.Spends);
            }

            /// <summary>
            /// counting monthly profit
            /// </summary>
            /// <returns></returns>
            public decimal MonthProfit()
            {
                return CountingMonthStats(Stats.Profit);
            }

            /// <summary>
            /// method to count monthly sum, filtered transactions depends on Stats
            /// </summary>
            /// <param name="stats">define count profit or spends</param>
            /// <returns>amount of monthly filtered transactions</returns>
            private decimal CountingMonthStats(Stats stats)
            {
                Func<decimal, bool> cond = null;
                switch (stats)
                {
                    case Stats.Profit:
                        cond = (x) => x > 0;
                        break;
                    case Stats.Spends:
                        cond = (x) => x < 0;
                        break;
                }

                DateTime dt = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                decimal sum = 0;
                foreach (Transaction transaction in Transactions)
                {
                    if (cond(transaction.Amount) && transaction.DateTime > dt) sum += transaction.Amount;
                }
                return sum;
            }

            private enum Stats
            {
                Spends,
                Profit
            }



        }

    }




}
