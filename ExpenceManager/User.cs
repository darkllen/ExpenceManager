using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenceManager
{
    public class User
    {
        public string name { get; }
        public string surname { get; }
        public string email { get; set; }
        private List<Wallet> wallets { get; }
        private List<Wallet> wallets_shared { get; }
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

        public int create_new_wallet(string name,
                                     double start_balance,
                                     string description,
                                     string currency)
        {
            Wallet wallet = new Wallet(name, start_balance, description, currency, categories);
            wallets.Add(wallet);
            return wallet.id;
        }

        public List<int> get_wallets_ids()
        {
            return convert_wallets_ids(wallets);
        }

        public List<int> get_shared_wallets_ids()
        {
            return convert_wallets_ids(wallets_shared);
        }

        public void share_wallet_with_user(int wallet_id, User user)
        {
            if (user == this) throw new UserException("can not share wallet with owner");
            Wallet to_share = get_own_wallet_by_id(wallet_id);
            if (to_share is null) throw new UserException("user hasn't this wallet");
            if (user.wallets_shared.Contains(to_share)) throw new UserException("wallet has been already shared with this user");
            user.wallets_shared.Add(to_share);
        }

        public void switch_category_permisiion(int wallet_id, Category category)
        {
            if (!categories.Contains(category)) throw new UserException("user hasn't this category");
            Wallet wallet = get_own_wallet_by_id(wallet_id);
            if (wallet is null) throw new UserException("user hasn't this wallet");
            if (wallet.restricted_categories.Contains(category)) wallet.restricted_categories.Remove(category);
            else wallet.restricted_categories.Add(category);
        }

        public void add_transaction(int wallet_id, Transaction transaction)
        {
            Wallet wallet = get_own_wallet_by_id(wallet_id);
            if (wallet is null) wallet = get_shared_wallet_by_id(wallet_id);
            if (wallet is null) throw new UserException("user hasn't this wallet");
            wallet.add_transaction(transaction);
        }

        public void remove_transaction(int wallet_id, Transaction transaction)
        {
            Wallet wallet = get_own_wallet_by_id(wallet_id);
            if (wallet is null) wallet = get_shared_wallet_by_id(wallet_id);
            if (wallet is null) throw new UserException("user hasn't this wallet");
            wallet.remove_transaction(transaction);
        }


        public List<Transaction> get_10_transactions(int wallet_id, int start)
        {
            Wallet wallet = get_own_wallet_by_id(wallet_id);
            if (wallet is null) wallet = get_shared_wallet_by_id(wallet_id);
            if (wallet is null) throw new UserException("user hasn't this wallet");
            return wallet.get_10_transactions(start);
        }

        public double get_wallet_ballance(int wallet_id)
        {
            Wallet wallet = get_own_wallet_by_id(wallet_id);
            if (wallet is null) wallet = get_shared_wallet_by_id(wallet_id);
            if (wallet is null) throw new UserException("user hasn't this wallet");
            return wallet.curr_balance;
        }

        public double get_this_month_profit(int wallet_id)
        {
            Wallet wallet = get_own_wallet_by_id(wallet_id);
            if (wallet is null) wallet = get_shared_wallet_by_id(wallet_id);
            if (wallet is null) throw new UserException("user hasn't this wallet");
            return wallet.month_adds();
        }


        public double get_this_month_spends(int wallet_id)
        {
            Wallet wallet = get_own_wallet_by_id(wallet_id);
            if (wallet is null) wallet = get_shared_wallet_by_id(wallet_id);
            if (wallet is null) throw new UserException("user hasn't this wallet");
            return wallet.month_spends();
        }




        private List<int> convert_wallets_ids(List<Wallet> wallets)
        {
            Converter<Wallet, int> map = (x) => x.id;
            return wallets.ConvertAll(map);
        }

        private Wallet get_own_wallet_by_id(int id)
        {
            return get_wallet_by_id(wallets, id);
        }

        private Wallet get_shared_wallet_by_id(int id)
        {
           return get_wallet_by_id(wallets_shared, id);
        }

        private Wallet get_wallet_by_id(List<Wallet> wallets, int id)
        {
            Predicate<Wallet> id_predicate = (x) => x.id == id;
            return wallets.Find(id_predicate);
        }



        public class UserException : Exception
        {
            public UserException(string message)
                : base(message)
            {
            }
        }

        public class WalletException : Exception
        {
            public WalletException(string message)
                : base(message)
            {
            }
        }


        private class Wallet
        {
            private static int next_id = 0;
            public int id { get; }
            private string name { get; set; }
            private double start_balance { get; }
            public double curr_balance { get; set; }
            public string description { get; set; }
            private string currency { get; }

            private List<Transaction> transactions;
            private List<Category> possible_categories;
            public List<Category> restricted_categories { get; }

            public Wallet(

                string name,
                double start_balance,
                string description,
                string currency,
                List<Category> possible_categories)
            {
                id = next_id++;
                this.name = name;
                this.start_balance = start_balance;
                curr_balance = start_balance;
                this.description = description;
                this.currency = currency;
                transactions = new List<Transaction>();
                this.possible_categories = possible_categories;
                restricted_categories = new List<Category>();
            }


            public List<Transaction> get_10_transactions(int start_place)
            {
                List<Transaction> returned_transactions = new List<Transaction>();
                for (int i = start_place; i < start_place + 10 && i < transactions.Count; i++)
                {
                    returned_transactions.Add(transactions[i]);
                }
                return returned_transactions;
            }

            public void add_transaction(Transaction transaction)
            {
                if (transactions.Contains(transaction)) throw new WalletException("this transaction already exists in this wallet");
                if (restricted_categories.Contains(transaction.category)) throw new WalletException("transaction category is restricted for this wallet");
                if (!possible_categories.Contains(transaction.category)) throw new WalletException("transaction category is not possible for this wallet");
                transactions.Add(transaction);
                curr_balance += transaction.amount;
            }
            public void remove_transaction(Transaction transaction)
            {
                transactions.Remove(transaction);
                curr_balance -= transaction.amount;
            }

            public double month_spends()
            {
                return counting_month_stats(Stats.Spends);
            }
            public double month_adds()
            {
                return counting_month_stats(Stats.Ads);
            }

            private double counting_month_stats(Stats stats)
            {
                Func<double, bool> cond = null;
                switch (stats)
                {
                    case Stats.Ads:
                        cond = (x) => x > 0;
                        break;
                    case Stats.Spends:
                        cond = (x) => x < 0;
                        break;
                }

                DateTime dt = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                double sum = 0;
                foreach (Transaction transaction in transactions)
                {
                    if (cond(transaction.amount) && transaction.date_time > dt) sum += transaction.amount;
                }
                return sum;
            }

            private enum Stats
            {
                Spends,
                Ads
            }



        }

    }




}
