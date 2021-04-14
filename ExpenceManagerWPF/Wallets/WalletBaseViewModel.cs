using ExpenceManagerWPF.Categories;
using ExpenceManagerWPF.Navigation;
using ExpenceManagerWPF.Transaction;

namespace ExpenceManagerWPF.Wallets
{
    public class WalletBaseViewModel : NavigationBase<WalletsNavigatableTypes>, INavigatable<MainNavigatableTypes>
    {
        
        public WalletBaseViewModel()
        {
            Navigate(WalletsNavigatableTypes.Wallet);
        }

        protected override INavigatable<WalletsNavigatableTypes> CreateViewModel(WalletsNavigatableTypes type)
        {
            if (type == WalletsNavigatableTypes.Wallet)
            {
                return new WalletsViewModel(() => Navigate(WalletsNavigatableTypes.WalletCreation), () => Navigate(WalletsNavigatableTypes.TransactionCreation), ()=>Navigate(WalletsNavigatableTypes.CategoryCreation), () => Navigate(WalletsNavigatableTypes.Wallet));
            }
            else if (type == WalletsNavigatableTypes.WalletCreation)
            {
                return new WalletCreateViewModel(() => Navigate(WalletsNavigatableTypes.Wallet));
            }
            else if (type == WalletsNavigatableTypes.CategoryCreation)
            {
                return new CategoryViewModel(() => Navigate(WalletsNavigatableTypes.Wallet));
            }
            else
            {
                return new TransactionCreateViewModel(() => Navigate(WalletsNavigatableTypes.Wallet));
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
