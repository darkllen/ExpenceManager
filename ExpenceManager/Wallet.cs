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
        private User owner {get;}

        private List<Transaction> transactions;
        private List<Category> categories;

        private Wallet(
            string name,
            double start_balance, 
            string description,
            string currency,
            User owner)
        {
            this.name = name;
            this.start_balance = start_balance;
            this.curr_balance = start_balance;
            this.description = description;
            this.currency = currency;
            this.owner = owner;
            transactions = new List<Transaction>();
        }

        public Wallet create_wallet_for_user(
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

        public void get_10_transactions(int start_place) {
            List<Transaction> returned_transactions = new List<Transaction>();
            for (int i = start_place; i<start_place+10 && i<transactions.len(); i++){
                returned_transactions.add(transactions[i]);
            }
            return returned_transactions;
        }

        public void add_transaction(Transaction transaction) {
            transactions.add(transaction);
        }
        public void remove_transaction(Transaction transaction) { 
            transactions.remove(transaction);
        }

        public void month_spends() {
            dt = new DateTime(DateTime.Today.year, DateTime.Today.month, 0);
            double sum = 0;
            foreach (Transaction transaction in transactions){
                if (transaction.amount< 0 && transaction.get_date_time> dt) sum+=transaction.amount;
            }
        }
        public void month_adds() {
            dt = new DateTime(DateTime.Today.year, DateTime.Today.month, 0);
            double sum = 0;
            foreach (Transaction transaction in transactions){
                if (transaction.amount > 0 && transaction.get_date_time> dt) sum+=transaction.amount;
            }
        }

    }
}
