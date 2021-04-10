using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.RightsManagement;
using System.Windows;
using ExpenceManager;
using ExpenceManagerModels.Wallet;
using ExpenceManagerWPF.Navigation;
using Prism.Commands;
using Prism.Mvvm;
using Services;

namespace ExpenceManagerWPF.Wallets
{
    public class WalletsViewModel : INotifyPropertyChanged, INavigatable<WalletsNavigatableTypes>
    {
        private Action _gotoWalletCreation;
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
                OnPropertyChanged();
            }
        }

        public DelegateCommand GoToCreation { get; }

        public DelegateCommand RemoveWalletCommand { get; }

        public WalletsViewModel(Action gotoWalletCreation)
        {
            Wallets = new ObservableCollection<WalletDetailsViewModel>();

            foreach (var wallet in AuthenticationService.CurrentUser.Wallets)
            {
                Wallets.Add(new WalletDetailsViewModel(wallet));
            }

            _gotoWalletCreation = gotoWalletCreation;
            GoToCreation = new DelegateCommand(_gotoWalletCreation);
            RemoveWalletCommand = new DelegateCommand(RemoveWallet);
        }


        public async void RemoveWallet()
        {
            WalletService service = new WalletService();
            WalletDB wallet = new WalletDB(_currentWallet.Guid, _currentWallet.Name, _currentWallet.CurrBalance, _currentWallet.Description,
                _currentWallet.Currency);
            await service.RemoveWallet(wallet);
            AuthenticationService.CurrentUser.Wallets.RemoveAll(x=>x.Guid==wallet.Guid);
            MessageBox.Show("Wallet was removed");
            Update();
        }


        public WalletsNavigatableTypes Type
        {
            get
            {
                return WalletsNavigatableTypes.Wallet;
            }
        }

        public void ClearSensitiveData()
        {
            
        }

        public void Update()
        {
            Wallets = new ObservableCollection<WalletDetailsViewModel>();
            foreach (var wallet in AuthenticationService.CurrentUser.Wallets)
            {
                Wallets.Add(new WalletDetailsViewModel(wallet));
            }
            OnPropertyChanged(nameof(Wallets));
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
