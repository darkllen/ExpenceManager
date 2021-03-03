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

        public void add_wallet(Wallet wallet)
        {
            if (wallet.owner==this && !wallets.Contains(wallet)){
                wallets.Add(wallet);
            }
        }

        public void share_wallet_with_user(Wallet wallet, User user)
        {
            if (wallets.Contains(wallet) && !user.wallets_shared.Contains(wallet)){
                user.wallets_shared.Add(wallet);
            } else {
            
            }
        }

        public void allow_category(Wallet wallet, Category category)
        {
            wallet.allow_category(category, this);
        }

        public void restrict_category(Wallet wallet, Category category)
        {
            wallet.restrict_category(category, this);
        }

    }
}
