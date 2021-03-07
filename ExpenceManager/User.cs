using System;
using System.Collections.Generic;

namespace ExpenceManager
{
    public class User
    {
        //user can't change name or surname
        public string name { get; }
        public string surname { get; }
        //user can change email
        public string email { get; set; }
        //wallets are private, so User methods are needed to be used to work with wallets
        private List<Wallet> wallets { get; }
        private List<Wallet> wallets_shared { get; }
        //categories are public, so we can change them easy
        public List<Category> categories { get; }

        public User(string name, string surname, string email)
        {   
            this.name = name;
            this.surname = surname;
            this.email = email;
            wallets = new List<Wallet>();
            wallets_shared = new List<Wallet>();
            categories = new List<Category>();
        }

        /// <summary>
        /// Create new wallet for User. 
        /// This is the only way to create wallet.
        /// </summary>
        /// <param name="name">wallet name</param>
        /// <param name="start_balance">wallet start ballance</param>
        /// <param name="description">wallet description</param>
        /// <param name="currency">wallet currency</param>
        /// <returns>id of created wallet</returns>
        public int create_new_wallet(string name,
                                     decimal start_balance,
                                     string description,
                                     string currency)
        {
            //create wallet and asign it to user
            Wallet wallet = new Wallet(name, start_balance, description, currency, categories);
            wallets.Add(wallet);
            return wallet.id;
        }

        /// <summary>
        /// get all wallets ids of user to use them later in User methods
        /// </summary>
        /// <returns>List with ids of wallets, which are owned by user</returns>
        public List<int> get_wallets_ids()
        {
            return convert_wallets_ids(wallets);
        }
        /// <summary>
        /// get all shared wallets ids of user to use them later in User methods
        /// </summary>
        /// <returns>List with ids of wallets, which are shared with user</returns>
        public List<int> get_shared_wallets_ids()
        {
            return convert_wallets_ids(wallets_shared);
        }

        /// <summary>
        /// share wallet with another user
        /// </summary>
        /// <param name="wallet_id">wallet id</param>
        /// <param name="user">user to share wallet with</param>
        public void share_wallet_with_user(int wallet_id, User user)
        {
            Wallet to_share = get_own_wallet_by_id(wallet_id);

            if (user == this) throw new UserException("can not share wallet with owner");
            if (to_share is null) throw new UserException("user hasn't this wallet");
            if (user.wallets_shared.Contains(to_share)) throw new UserException("wallet has been already shared with this user");

            user.wallets_shared.Add(to_share);
        }

        /// <summary>
        /// change categories accessebility for wallet.
        /// </summary>
        /// <param name="wallet_id">wallet id</param>
        /// <param name="category">category to change</param>
        public void switch_category_permisiion(int wallet_id, Category category)

        {
            Wallet wallet = get_own_wallet_by_id(wallet_id);

            if (!categories.Contains(category)) throw new UserException("user hasn't this category");
            if (wallet is null) throw new UserException("user hasn't this wallet");

            if (wallet.restricted_categories.Contains(category)) wallet.restricted_categories.Remove(category);
            else wallet.restricted_categories.Add(category);
        }

        /// <summary>
        /// add transaction to wallet
        /// </summary>
        /// <param name="wallet_id">wallet id</param>
        /// <param name="transaction">transaction to add</param>
        public void add_transaction(int wallet_id, Transaction transaction)
            
        {
            get_wallet_or_exception(wallet_id).add_transaction(transaction);
        }

        /// <summary>
        /// remove transaction from wallet
        /// </summary>
        /// <param name="wallet_id">wallet id</param>
        /// <param name="transaction">transaction to remove</param>
        public void remove_transaction(int wallet_id, Transaction transaction)
        {
            get_wallet_or_exception(wallet_id).remove_transaction(transaction);
        }

        /// <summary>
        /// get 10 first transactions starts from certain position
        /// </summary>
        /// <param name="wallet_id">wallet id</param>
        /// <param name="start">position to start from</param>
        /// <returns>List with 10 or less transactions</returns>
        public List<Transaction> get_10_transactions(int wallet_id, int start)
        {
            return get_wallet_or_exception(wallet_id).get_10_transactions(start);
        }

        /// <summary>
        /// show wallet current balance
        /// </summary>
        /// <param name="wallet_id">wallet id</param>
        /// <returns>curent balance of wallet</returns>
        public decimal get_wallet_ballance(int wallet_id)
        {
            return get_wallet_or_exception(wallet_id).curr_balance;
        }

        /// <summary>
        /// get profit for this wallet started from the first day of current month
        /// </summary>
        /// <param name="wallet_id">walet id</param>
        /// <returns>amount of profit</returns>
        public decimal get_this_month_profit(int wallet_id)
        {
            return get_wallet_or_exception(wallet_id).month_profit();
        }

        /// <summary>
        /// get spends for this wallet started from the first day of current month
        /// </summary>
        /// <param name="wallet_id">wallet id</param>
        /// <returns>amount of spends</returns>
        public decimal get_this_month_spends(int wallet_id)
        {
            return get_wallet_or_exception(wallet_id).month_spends();
        }



        /// <summary>
        /// convert List of Wallet to List of Wallet.id
        /// </summary>
        /// <param name="wallets">List of wallets></param>
        /// <returns>List of ids</returns>
        private List<int> convert_wallets_ids(List<Wallet> wallets)
        {
            Converter<Wallet, int> map = (x) => x.id;
            return wallets.ConvertAll(map);
        }

        /// <summary>
        /// get Wallet from own wallets by id
        /// </summary>
        /// <param name="id">wallet id</param>
        /// <returns>wallet with this id</returns>
        private Wallet get_own_wallet_by_id(int id)
        {
            return get_wallet_by_id(wallets, id);
        }

        /// <summary>
        /// get Wallet from shared wallets by id
        /// </summary>
        /// <param name="id">wallet id</param>
        /// <returns>wallet with this id</returns>
        private Wallet get_shared_wallet_by_id(int id)
        {
           return get_wallet_by_id(wallets_shared, id);
        }

        /// <summary>
        /// find Wallet with certain id from List
        /// </summary>
        /// <param name="wallets">List of wallets</param>
        /// <param name="id">wallet id</param>
        /// <returns>wallet with this id</returns>
        private Wallet get_wallet_by_id(List<Wallet> wallets, int id)
        {
            Predicate<Wallet> id_predicate = (x) => x.id == id;
            return wallets.Find(id_predicate);
        }

        /// <summary>
        /// search Wallet with id among all wallets of this user (own and shared)
        /// throws UserException id there is no such Wallet
        /// </summary>
        /// <param name="wallet_id">wallet id</param>
        /// <returns>wallet with this id</returns>
        private Wallet get_wallet_or_exception(int wallet_id)
        {
            Wallet wallet = get_own_wallet_by_id(wallet_id);
            if (wallet is null) wallet = get_shared_wallet_by_id(wallet_id);
            if (wallet is null) throw new UserException("user hasn't this wallet");
            return wallet;
        }

        private class Wallet
        {
            // field to imitate autogenerated id
            private static int next_id = 0;
            public int id { get; }
            private string name { get; set; }
            private decimal start_balance { get; }
            public decimal curr_balance { get; set; }
            public string description { get; set; }
            private string currency { get; }

            private List<Transaction> transactions;
            private List<Category> possible_categories;
            public List<Category> restricted_categories { get; }

            public Wallet(

                string name,
                decimal start_balance,
                string description,
                string currency,
                List<Category> possible_categories)
            {
                this.name = name;
                this.start_balance = start_balance;
                this.description = description;
                this.currency = currency;
                //possible_categories of Wallet are always actual as they changed with any changes in User categories
                this.possible_categories = possible_categories;

                id = next_id++;
                curr_balance = start_balance;

                transactions = new List<Transaction>();
                restricted_categories = new List<Category>();
            }

            /// <summary>
            /// get 10 first transactions starts from certain position
            /// </summary>
            /// <param name="start_place">position to start from</param>
            /// <returns>List with 10 or less transactions</returns>
            public List<Transaction> get_10_transactions(int start_place)
            {
                List<Transaction> returned_transactions = new List<Transaction>();
                for (int i = start_place; i < start_place + 10 && i < transactions.Count; i++)
                {
                    returned_transactions.Add(transactions[i]);
                }
                return returned_transactions;
            }

            /// <summary>
            /// add transaction to wallet if possible
            /// </summary>
            /// <param name="transaction">transaction to add</param>
            public void add_transaction(Transaction transaction)
            {
                if (transactions.Contains(transaction)) throw new WalletException("this transaction already exists in this wallet");
                if (restricted_categories.Contains(transaction.category)) throw new WalletException("transaction category is restricted for this wallet");
                if (!possible_categories.Contains(transaction.category)) throw new WalletException("transaction category is not possible for this wallet");
                transactions.Add(transaction);
                curr_balance += transaction.amount;
            }

            /// <summary>
            /// remove transaction from wallet
            /// </summary>
            /// <param name="transaction">transaction to remove</param>
            public void remove_transaction(Transaction transaction)
            {
                transactions.Remove(transaction);
                curr_balance -= transaction.amount;
            }

            /// <summary>
            /// counting monthly spends
            /// </summary>
            /// <returns></returns>
            public decimal month_spends()
            {
                return counting_month_stats(Stats.Spends);
            }

            /// <summary>
            /// counting monthly profit
            /// </summary>
            /// <returns></returns>
            public decimal month_profit()
            {
                return counting_month_stats(Stats.Profit);
            }

            /// <summary>
            /// method to count monthly sum, filtered transactions depends on Stats
            /// </summary>
            /// <param name="stats">define count profit or spends</param>
            /// <returns>amount of monthly filtered transactions</returns>
            private decimal counting_month_stats(Stats stats)
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
                foreach (Transaction transaction in transactions)
                {
                    if (cond(transaction.amount) && transaction.date_time > dt) sum += transaction.amount;
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
