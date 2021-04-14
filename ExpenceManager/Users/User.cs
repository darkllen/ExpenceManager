using System;
using System.Collections.Generic;
using DataStorage;

namespace ExpenceManager
{
    public class User : IStorable
    {
        //user can't change name or surname
        public Guid Guid { get; }
        public string Name { get; set; }
        public string Surname { get; set; }
        //user can change email
        public string Email { get; set; }
        //wallets are private, so User methods are needed to be used to work with wallets
        public List<Wallet> Wallets { get; }
        public List<Wallet> WalletsShared { get; }
        //categories are public, so we can change them easy
        public List<Category> Categories { get; }

        public User(string name, string surname, string email, Guid guid, List<Category> categories)
        {   
            Name = name;
            Surname = surname;
            Email = email;
            Guid = guid;
            Wallets = new List<Wallet>();
            WalletsShared = new List<Wallet>();

            if (!categories.Contains(Category.DefaultCategory))categories.Add(Category.DefaultCategory);
            Categories = categories;
        }

        public User(string name, string surname, string email)
        {
            Name = name;
            Surname = surname;
            Email = email;
            Guid = new Guid();
            Wallets = new List<Wallet>();
            WalletsShared = new List<Wallet>();
            Categories = new List<Category>() {Category.DefaultCategory};
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
        public Wallet CreateNewWallet(string name,
                                   decimal startBalance,
                                   string description,
                                   string currency)
        {
            //create wallet and asign it to user
            Wallet wallet = Wallet.CreateWalletForUser(this, name, startBalance, description, currency);
            return wallet;
        }



        /// <summary>
        /// share wallet with another user
        /// </summary>
        /// <param name="walletId">wallet id</param>
        /// <param name="user">user to share wallet with</param>
        public void ShareWalletWithUser(Wallet toShare, User user)
        {
            if (!Wallets.Contains(toShare)) throw new UserException("can not share wallet if you are not owner");
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
        public void SwitchCategoryPermisiion(Wallet wallet, Category category)

        {
            if (!Wallets.Contains(wallet)) throw new UserException("can not switch categories if you are not owner");
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
        public void AddTransaction(Wallet wallet, Transaction transaction)
            
        {
            wallet.AddTransaction(this, transaction);
        }

        /// <summary>
        /// remove transaction from wallet
        /// </summary>
        /// <param name="walletId">wallet id</param>
        /// <param name="transaction">transaction to remove</param>
        public void RemoveTransaction(Wallet wallet, Transaction transaction)
        {
            wallet.RemoveTransaction(this,  transaction);
        }

        /// <summary>
        /// get 10 first transactions starts from certain position
        /// </summary>
        /// <param name="walletId">wallet id</param>
        /// <param name="start">position to start from</param>
        /// <returns>List with 10 or less transactions</returns>
        public List<Transaction> Get10Transactions(Wallet wallet, int start)
        {
            return wallet.Get10Transactions(this, start);
        }

        /// <summary>
        /// show wallet current balance
        /// </summary>
        /// <param name="walletId">wallet id</param>
        /// <returns>curent balance of wallet</returns>
        public decimal GetWalletBallance(Wallet wallet)
        {
            return wallet.CurrBalance;
        }

        /// <summary>
        /// get profit for this wallet started from the first day of current month
        /// </summary>
        /// <param name="walletId">walet id</param>
        /// <returns>amount of profit</returns>
        public decimal GetThisMonthProfit(Wallet wallet)
        {
            return wallet.MonthProfit(this);
        }

        /// <summary>
        /// get spends for this wallet started from the first day of current month
        /// </summary>
        /// <param name="walletId">wallet id</param>
        /// <returns>amount of spends</returns>
        public decimal GetThisMonthSpends(Wallet wallet)
        {
            return wallet.MonthSpends(this);
        }





    }




}
