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

        private List<Transaction> transactions;
        private List<Category> categories;

        private Wallet(
            string name,
            double start_balance, 
            string description,
            string currency)
        {
            this.name = name;
            this.start_balance = start_balance;
            this.curr_balance = start_balance;
            this.description = description;
            this.currency = currency;
            transactions = new List<Transaction>();
        }

        public void month_spends() { }
        public void month_adds() { }

        public Wallet create_wallet_for_user(
            string name,
            double start_balance,
            string description,
            string currency,
            User user)
        {
            return new Wallet(name, start_balance, description, currency);
        }

        public void get_transactions() { }
        public void add_transaction() { }
        public void remove_transaction() { }
        public void edit_transaction() { }

    }
}
