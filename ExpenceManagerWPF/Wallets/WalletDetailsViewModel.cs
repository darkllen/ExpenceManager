using System;
using System.Windows;
using ExpenceManager;
using ExpenceManagerModels.Wallet;
using Prism.Commands;
using Prism.Mvvm;
using Services;

namespace ExpenceManagerWPF.Wallets
{
    public class WalletDetailsViewModel : BindableBase
    {
        private Wallet _wallet;

        public string Name
        {
            get
            {
                return _wallet.Name;
            }
            set
            {
                _wallet.Name = value;
                RaisePropertyChanged(nameof(DisplayName));
            }
        }

        public Guid Guid
        {
            get
            {
                return _wallet.Guid;
            }

        }

        public DelegateCommand UpdateWalletCommand { get; }

        public string Description
        {
            get
            {
                return _wallet.Description;
            }
            set
            {
                _wallet.Description = value;
                RaisePropertyChanged(nameof(DisplayName));
            }
        }

        public decimal CurrBalance
        {
            get
            {
                return _wallet.CurrBalance;
            }

        }



        public string Currency
        {
            get
            {
                return _wallet.Currency;
            }
        }

        public string DisplayName
        {
            get
            {
                return $"{_wallet.Name} (${_wallet.CurrBalance})";
            }
        }

        public WalletDetailsViewModel(Wallet wallet)
        {
            _wallet = wallet;
            UpdateWalletCommand = new DelegateCommand(updateWallet);
        }


        private async void updateWallet()
        {

            var service = new WalletService();
            try
            {
                WalletDB wallet = new WalletDB(_wallet.Guid, _wallet.Name, _wallet.CurrBalance, _wallet.Description,
                    _wallet.Currency);
                await service.SaveUpdateWallet(AuthenticationService.CurrentUser, wallet);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Update wallet failed: {ex.Message}");
                return;
            }

            MessageBox.Show($"Wallet updated");

        }
    }
}
