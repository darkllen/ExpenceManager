using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenceManager
{
    public class User
    {
        private string name { get; }
        private string surname { get; }
        private string email { get; set; }
        private List<Wallet> wallets { get; }
        private List<Wallet> wallets_shared { get; }
        private List<Category> categories { get; }

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
            if (wallet.get_owner()==this && !wallets.contains(wallet)){
                wallets.add(wallet);
            }
        }

        public void share_wallet_with_user(Wallet wallet, User user)
        {
            if (wallets.contains(wallet) && !user.wallets_shared.contains(wallet)){
                user.wallets_shared.add(wallet);
            } else {
            
            }
        }

        public void allow_category(Wallet wallet, Category category)
        {

        }

        public void restrict_category(Wallet wallet, Category category)
        {

        }



    }
}
