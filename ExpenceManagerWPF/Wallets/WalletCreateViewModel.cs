using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ExpenceManager;
using ExpenceManagerModels.Wallet;
using Services;
using ExpenceManagerWPF.Navigation;
using Prism.Commands;

namespace ExpenceManagerWPF.Wallets
{
    
    class WalletCreateViewModel :  INotifyPropertyChanged, INavigatable<WalletsNavigatableTypes>
    {
        private WalletDB _wallet = new WalletDB();
        private Action _gotoWallets;

        public DelegateCommand GoBackCommand { get; }
        public DelegateCommand AddWalletCommand { get; }
        public WalletCreateViewModel(Action gotoWallets)
        {
            _gotoWallets = gotoWallets;
            GoBackCommand = new DelegateCommand(_gotoWallets);
            AddWalletCommand = new DelegateCommand(CreateWallet);
        }

        public string Name
        {
            get
            {
                return _wallet.Name;
            }
            set
            {
                _wallet.Name = value;
                OnPropertyChanged();
            }
        }

        public decimal StartBalance
        {
            get
            {
                return _wallet.CurrBalance;
            }
            set
            {
                _wallet.CurrBalance = value;
                OnPropertyChanged();
            }
        }

        public string Description
        {
            get
            {
                return _wallet.Description;
            }
            set
            {
                _wallet.Description = value;
                OnPropertyChanged();
            }
        }

        public string Currency
        {
            get
            {
                return _wallet.Currency;
            }
            set
            {
                _wallet.Currency = value;
                OnPropertyChanged();
            }
        }


        private async void CreateWallet()
        {

            var service = new WalletService();
            try
            {
                AuthenticationService.CurrentUser.CreateNewWallet(_wallet.Name, _wallet.CurrBalance, _wallet.Description, _wallet.Currency);
                await service.SaveUpdateWallet(AuthenticationService.CurrentUser, _wallet);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Add walletfailed: {ex.Message}");
                return;
            }

            MessageBox.Show($"Wallet added");
            _gotoWallets.Invoke();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public WalletsNavigatableTypes Type
        {
            get
            {
                return WalletsNavigatableTypes.Creation;
            }
        }
        public void ClearSensitiveData()
        {
            _wallet = new WalletDB();
        }

        public void Update()
        {
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
