using System.Collections.ObjectModel;
using System.Security.RightsManagement;
using ExpenceManager;
using ExpenceManagerWPF.Navigation;
using Prism.Mvvm;

namespace ExpenceManagerWPF.Wallets
{
    public class WalletsViewModel : BindableBase, INavigatable<MainNavigatableTypes>
    {
       // private WalletService _service;
        private WalletDetailsViewModel _currentWallet;
        public ObservableCollection<WalletDetailsViewModel> Wallets { get; set; }

        public WalletDetailsViewModel CurrentWallet
        {
            get
            {
                return _currentWallet;
            }
            set
            {
                _currentWallet = value;
                RaisePropertyChanged();
            }
        }

        public WalletsViewModel()
        {
            User u = new User("a", "a", "a");
            Wallet w =  u.CreateNewWallet("a", 10, "a", "a");

            Wallets = new ObservableCollection<WalletDetailsViewModel>();
            Wallets.Add(new WalletDetailsViewModel(w));
            // _service = new WalletService();
            // Wallets = new ObservableCollection<WalletDetailsViewModel>();
            // foreach (var wallet in _service.GetWallets())
            // {
            //     Wallets.Add(new WalletDetailsViewModel(wallet));
            // }
        }


        public MainNavigatableTypes Type 
        {
            get
            {
                return MainNavigatableTypes.Wallets;
            }
        }
        public void ClearSensitiveData()
        {
            
        }
    }
}
