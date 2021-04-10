using System;
using ExpenceManagerWPF.Navigation;

namespace ExpenceManagerWPF.Wallets
{
    public class WalletBaseViewModel : NavigationBase<WalletsNavigatableTypes>, INavigatable<MainNavigatableTypes>
    {
        private Action _signInSuccess;


        public WalletBaseViewModel()
        {
            Navigate(WalletsNavigatableTypes.Wallet);
        }

        protected override INavigatable<WalletsNavigatableTypes> CreateViewModel(WalletsNavigatableTypes type)
        {
            if (type == WalletsNavigatableTypes.Wallet)
            {
                return new WalletsViewModel(() => Navigate(WalletsNavigatableTypes.Creation));
            }
            else
            {
                return new WalletCreateViewModel(() => Navigate(WalletsNavigatableTypes.Wallet));
            }
        }

        public MainNavigatableTypes Type
        {
            get
            {
                return MainNavigatableTypes.Auth;
            }
        }

        public void ClearSensitiveData()
        {
            CurrentViewModel.ClearSensitiveData();
        }

        public void Update()
        {
            CurrentViewModel.Update();
        }
    }
}
