using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenceManager
{
    public class Wallet
    {
        private string name { get; set; }
        private double start_balance { get; }
        private double curr_balance { get; }
        private string description { get; set; }
        private string currency { get; }
        public User owner {get;}

        private List<Transaction> transactions;
        private List<Category> possible_categories;
        private List<Category> restricted_categories;

        private Wallet(
            string name,
            double start_balance, 
            string description,
            string currency,
            User owner)
        {
            this.name = name;
            this.start_balance = start_balance;
            curr_balance = start_balance;
            this.description = description;
            this.currency = currency;
            this.owner = owner;
            transactions = new List<Transaction>();
            possible_categories = owner.categories;
            restricted_categories = new List<Category>();
        }

        public static Wallet create_wallet_for_user(
            string name,
            double start_balance,
            string description,
            string currency,
            User user)
        {
            Wallet new_wallet = new Wallet(name, start_balance, description, currency, user);
            user.add_wallet(new_wallet);
            return new_wallet;
        }

        public void restrict_category(Category category, User user)
        {
            if (owner == user && possible_categories.Contains(category) && !restricted_categories.Contains(category))
            {
                restricted_categories.Add(category);   
            }
        }

        public void allow_category(Category category, User user)
        {
            if (owner == user && possible_categories.Contains(category) && restricted_categories.Contains(category))
            {
                restricted_categories.Remove(category);
            }
        }

        public List<Transaction> get_10_transactions(int start_place) {
            List<Transaction> returned_transactions = new List<Transaction>();
            for (int i = start_place; i<start_place+10 && i<transactions.Count; i++){
                returned_transactions.Add(transactions[i]);
            }
            return returned_transactions;
        }

        public void add_transaction(Transaction transaction) {
            transactions.Add(transaction);
        }
        public void remove_transaction(Transaction transaction) { 
            transactions.Remove(transaction);
        }

        public void month_spends() {
            DateTime dt = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 0);
            double sum = 0;
            foreach (Transaction transaction in transactions){
                if (transaction.amount< 0 && transaction.date_time> dt) sum+=transaction.amount;
            }
        }
        public void month_adds() {
            DateTime dt = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 0);
            double sum = 0;
            foreach (Transaction transaction in transactions){
                if (transaction.amount > 0 && transaction.date_time > dt) sum+=transaction.amount;
            }
        }

    }
}
