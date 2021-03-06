﻿using System;
using System.Collections.Generic;
using System.Linq;
using DataStorage;
using ExpenceManagerModels.Users;

namespace ExpenceManagerModels.Wallet
{

    public class Wallet : IStorable
    {
        public static Dictionary<string, decimal> PossibleCurrency = new Dictionary<string, decimal>
        {
            {"UAH", 1},
            {"USD", 28},
            {"CNY", (decimal) 4.28}
        };

        public Guid Guid { get; }
        public string Name { get; set; }

        public decimal StartBalance { get; set; }
        public decimal CurrBalance { get; set; }
        public string Description { get; set; }


        private string _currency;
        public string Currency
        {
            get => _currency;

            set
            {
                StartBalance = Transaction.ConvertTo(_currency, value, StartBalance);
                _currency = value;
                CurrBalance = Transactions.Aggregate(StartBalance, (x, y) => x + y.ConvertTo(_currency));
            }
        }

        private List<Transaction> Transactions;
        private List<Category> PossibleCategories;
        public List<Category> RestrictedCategories { get; }


        public static Wallet CreateWalletForUser(
            User user,
            string name,
            decimal balance,
            string description,
            string currency)
        {
            Wallet wallet = new Wallet(name, balance, description, currency, user.Categories);
            user.Wallets.Add(wallet);
            return wallet;
        }

        public static Wallet CreateWalletForUser(
            User user,
            string name,
            decimal balance,
            string description,
            string currency, 
            Guid guid)
        {
            Wallet wallet = new Wallet(name, balance, description, currency, user.Categories, guid);
            user.Wallets.Add(wallet);
            return wallet;
        }

        private Wallet(

            string name,
            decimal balance,
            string description,
            string currency,
            List<Category> possibleCategories)
        {
            Name = name;
            Description = description;
            _currency = currency;
            //possibleCategories of Wallet are always actual as they changed with any changes in User categories
            PossibleCategories = possibleCategories;

            CurrBalance = balance;
            StartBalance = balance;

            Guid = Guid.NewGuid();
            Transactions = new List<Transaction>();
            RestrictedCategories = new List<Category>();
        }

        private Wallet(

            string name,
            decimal balance,
            string description,
            string currency,
            List<Category> possibleCategories, 
            Guid guid)
        {
            Name = name;
            Description = description;
            _currency = currency;
            //possibleCategories of Wallet are always actual as they changed with any changes in User categories
            PossibleCategories = possibleCategories;

            CurrBalance = balance;
            StartBalance = balance;
            Guid = guid;
            Transactions = new List<Transaction>();
            RestrictedCategories = new List<Category>();
        }

        /// <summary>
        /// get 10 first transactions starts from certain position
        /// </summary>
        /// <param name="user"></param>
        /// <param name="startPlace">position to start from</param>
        /// <returns>List with 10 or less transactions</returns>
        public List<Transaction> Get10Transactions(User user, int startPlace)
        {
            CheckRightsAny(user);
            List<Transaction> returnedTransactions = new List<Transaction>();
            for (int i = startPlace; i < startPlace + 10 && i < Transactions.Count; i++)
            {
                returnedTransactions.Add(Transactions[i]);
            }
            return returnedTransactions;
        }

        public List<Transaction> GetAllTransactions(User user)
        {
            CheckRightsAny(user);
            return Transactions;
        }

        /// <summary>
        /// add transaction to wallet if possible
        /// </summary>
        /// <param name="user"></param>
        /// <param name="transaction">transaction to add</param>
        public void AddTransaction(User user, Transaction transaction)
        {
            CheckRightsAny(user);
            if (Transactions.Contains(transaction)) throw new WalletException("this transaction already exists in this wallet");
            if (RestrictedCategories.Contains(transaction.Category)) throw new WalletException("transaction category is restricted for this wallet");
            if (!PossibleCategories.Contains(transaction.Category)) throw new WalletException("transaction category is not possible for this wallet");
            Transactions.Add(transaction);
            CurrBalance += transaction.ConvertTo(_currency);
        }

        /// <summary>
        /// remove transaction from wallet
        /// </summary>
        /// <param name="user"></param>
        /// <param name="transaction">transaction to remove</param>
        public void RemoveTransaction(User user, Transaction transaction)
        {
            CheckRightsAny(user);

            Transactions.Remove(transaction);
            CurrBalance = Transactions.Aggregate(StartBalance, (x, y) => x + y.ConvertTo(_currency));

        }

        /// <summary>
        /// counting monthly spends
        /// </summary>
        /// <returns></returns>
        public decimal MonthSpends(User user)
        {
            CheckRightsAny(user);
            return CountingMonthStats(Stats.Spends);
        }

        /// <summary>
        /// counting monthly profit
        /// </summary>
        /// <returns></returns>
        public decimal MonthProfit(User user)
        {
            CheckRightsAny(user);
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
                if (cond(transaction.Amount) && transaction.DateTime >= dt) sum += transaction.ConvertTo(_currency);
            }
            return sum;
        }

        public void CheckRightsAny(User user)
        {
            if (!user.Wallets.Contains(this) && !user.WalletsShared.Contains(this)) throw new UserException("user has no rights for this operation");
        }

        private enum Stats
        {
            Spends,
            Profit
        }



    }

}