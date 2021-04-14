using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
            AddWalletCommand = new DelegateCommand(CreateWallet, IsValid);
        }

        public List<string> PossibleCurrency => Wallet.PossibleCurrency.Keys.ToList();

        public string NameErr { get; set; }
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
                AddWalletCommand.RaiseCanExecuteChanged();
            }
        }

        public string BalanceErr { get; set; }
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
                AddWalletCommand.RaiseCanExecuteChanged();
            }
        }

        public string DescriptionErr { get; set; }
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
                AddWalletCommand.RaiseCanExecuteChanged();
            }
        }

        public string CurrencyErr { get; set; }
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
                AddWalletCommand.RaiseCanExecuteChanged();
            }
        }


        private bool IsValid()
        {
            bool valid = true;
            if (String.IsNullOrWhiteSpace(Name))
            {
                NameErr = "Name can't be empty";
                OnPropertyChanged(nameof(NameErr));
                valid = false;
            } else if (Name.Length > 20)
            {
                NameErr = "Name can't be more than 20 symbols";
                OnPropertyChanged(nameof(NameErr));
                valid = false;
            }
            else
            {
                NameErr = "";
                OnPropertyChanged(nameof(NameErr));
            }

            if (String.IsNullOrWhiteSpace(Description))
            {
                DescriptionErr = "Description can't be empty";
                OnPropertyChanged(nameof(DescriptionErr));
                valid = false;
            }
            else
            {
                DescriptionErr = "";
                OnPropertyChanged(nameof(DescriptionErr));
            }

            if (String.IsNullOrWhiteSpace(Currency))
            {
                CurrencyErr = "Choose currency";
                OnPropertyChanged(nameof(CurrencyErr));
                valid = false;
            }
            else 
            {
                CurrencyErr = "";
                OnPropertyChanged(nameof(CurrencyErr));
            }

            if (StartBalance < 0)
            {
                BalanceErr = "Start balance can't be less than 0";
                OnPropertyChanged(nameof(BalanceErr));
                valid = false;
            }
            else
            {
                BalanceErr = "";
                OnPropertyChanged(nameof(BalanceErr));
            }

            return valid;
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
                MessageBox.Show($"Add wallet failed: {ex.Message}");
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
                return WalletsNavigatableTypes.WalletCreation;
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
